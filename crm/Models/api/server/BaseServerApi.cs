﻿using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using crm.Models.user;
using crm.Models.api.server.valuesconverter;
using Newtonsoft.Json;

namespace crm.Models.api.server
{
    public abstract class BaseServerApi
    {

        #region const
        Dictionary<string, string> serverErrorsDictionary = new Dictionary<string, string>()
        {
            {"auth-0001", "Токен не найден"},
            {"auth-0002", "Неопознанный токен"},
            {"auth-0003", "Доступ запрещен"},
            {"auth-0004", "Не задан адрес электронной почты"},
            {"auth-0005", "Не указан пароль"},
            {"auth-0006", "Не задано имя пользователя"},
            {"auth-0007", "Не указана дата рождения"},
            {"auth-0008", "Не указан номер телефона"},
            {"auth-0009", "Не указан адрес телеграм"},
            {"auth-0010", "Не указан номер кошелька"},
            {"auth-0011", "Не указаны устройства"},
            {"auth-0012", "Пароль не соответствет требованиям"},
            {"auth-0013", "Пароль должен содержать более 8 символов"},
            {"auth-0014", "Пароль должен содержать заглавные, строчные буквы и цифры"},
            {"auth-0015", "Электронный адрес должен принадлежать protonmail.com"},
            {"auth-0016", "ID должен быть целым числом"},
            {"auth-0050", "Не удалось хэшировать пароль"},
            {"auth-0051", "Пользователь уже существует"},
            {"auth-0052", "Не удалось создать пользователя"},
            {"auth-0053", "Не удалось добавить права пользователю"},
            {"auth-0054", "Не удалось получить список пользователей"},
            {"auth-0055", "Нет зарегистрированных пользователей"},
            {"auth-0056", "Пользователь на найден"},
            {"auth-0057", "Не удалось получить ID пользователя"},
            {"auth-0058", "Пользователь с таким адресом не зарегистрирован"},
            {"auth-0059", "Не удалось создать токен доступа"},
            {"auth-0060", "Неверный пароль"},
            {"auth-0061", "Не удалось получить ID из токена"},
            {"auth-0062", "Не удалось получить пользователя по ID"}

        };
        
        #endregion

        #region vars
        protected string url;
        IConverter converter;
        #endregion

        public BaseServerApi(string url)
        {
            this.url = url;
            converter = new Converter();
        }

        #region helpers
        string getErrMsg(List<ServerError> errs)
        {
            string res = "";
            foreach (var item in errs)
            {
                if (serverErrorsDictionary.ContainsKey(item.code))
                    res += $"{serverErrorsDictionary[item.code]}\n";
                else
                    res += $"{item.code} {item.message}\n";
            }
            return res;
        }
        #endregion

        #region public
        public virtual async Task<bool> ValidateRegToken(string token)
        {
            bool res = false;
            try
            {
                await Task.Run(() => {
                    var client = new RestClient($"{url}/v1/auth/validateRegToken");
                    var request = new RestRequest(Method.POST);
                    request.AddHeader($"Authorization", $"Bearer {token}");
                    IRestResponse response = client.Execute(request);
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new ServerResponseException(response.StatusCode);
                    JObject json = JObject.Parse(response.Content);
                    res = json["success"].ToObject<bool>();
                    if (res == null)
                        throw new NoDataReceivedException();
                    if (!res)                    
                        throw new ApiException("Невалидный токен или его срок действия истек");                                        
                });

            }  catch (Exception ex)
            {
                throw new ApiException(ex.Message); 
            }  
            return res;
        }

        public virtual async Task<bool> RegisterUser(string token, BaseUser user)
        {
            bool res = false;

            await Task.Run(() => { 
                var client = new RestClient($"{url}/v1/users");
                var request = new RestRequest(Method.POST);
                request.AddHeader($"Authorization", $"Bearer {token}");
                dynamic p = new JObject();
                p.email = user.Email;
                p.password = user.Password;
                p.userfullname = user.FullName;
                p.birthday = converter.date(user.BirthDate, Direction.user_server);
                p.phone = converter.phone(user.PhoneNumber, Direction.user_server);
                p.telegram = converter.telegram(user.Telegram, Direction.user_server);
                p.usdt_account = user.Wallet;
                p.firstname = user.FirstName;
                p.middlename = user.MiddleName;
                p.lastname = user.LastName;
                p.fullname = $"{user.LastName} {user.FirstName} {user.MiddleName}";
                //p.devices = new JArray();
                //foreach (var device in user.Devices) 
                //    p.devices.Add(device);
                p.device = user.Devices[0].Name;
                request.AddParameter("application/json", p.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful)
                    throw new ServerResponseException(response.StatusCode);
                JObject json = JObject.Parse(response.Content);
                res = json["success"].ToObject<bool>();
                if (res == null)
                    throw new NoDataReceivedException();
                if (!res)
                    throw new ApiException("Не удалось зарегистрировать пользователя");

            });

            return res;
        }

        public virtual async Task<BaseUser> Login(string login, string password)
        {
            User user = null;

            await Task.Run(async () => { 

                var client = new RestClient($"{url}/v1/auth");
                var request = new RestRequest(Method.POST);            
                dynamic p = new JObject();
                p.email = login;
                p.password = password;
                request.AddParameter("application/json", p.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                JObject json = JObject.Parse(response.Content);
                if (!response.IsSuccessful)
                {
                    string e = json["errors"].ToString();
                    List<ServerError>? errors = JsonConvert.DeserializeObject<List<ServerError>>(e);
                    throw new ServerException($"{getErrMsg(errors)}");
                }
                
                bool res = json["success"].ToObject<bool>();
                string token, id;
                

                if (res)
                {
                    token = json["data"]["token"].ToObject<string>();
                    id = json["data"]["userID"].ToObject<string>();                    
                } else
                {
                    string e = json["errors"].ToString();
                    List<ServerError>? errors = JsonConvert.DeserializeObject<List<ServerError>>(e);
                    throw new ServerException($"{getErrMsg(errors)}");
                }

                if (res)
                {
                    user = await GetUser(id, token);    
                }

            });
            return user;

        }

        public virtual async Task<User> GetUser(string id, string token)
        {
            User user = new User();

            await Task.Run(() => {

                var client = new RestClient($"{url}/v1/users/{id}");
                var request = new RestRequest(Method.GET);
                request.AddHeader($"Authorization", $"Bearer {token}");
                var response = client.Execute(request);
                var json = JObject.Parse(response.Content);
                var res = json["success"].ToObject<bool>();
                if (res)
                {
                    JToken data = json["data"];
                    if (data != null)
                    {                        
                        user = JsonConvert.DeserializeObject<User>(data.ToString());
                    }

                } else
                {
                    string e = json["errors"].ToString();
                    List<ServerError>? errors = JsonConvert.DeserializeObject<List<ServerError>>(e);
                    throw new ServerException($"{getErrMsg(errors)}");
                }
            });

            user.Id = id;
            user.Token = token;

            return user;
        }
        
        public virtual async Task<(List<User>, int)> GetUsers(int page, int size, string token)
        {
            List<User> users = new List<User>();
            int total_pages = 0;

            var client = new RestClient($"{url}/v1/users/");
            var request = new RestRequest(Method.GET);
            request.AddHeader($"Authorization", $"Bearer {token}");
            request.AddQueryParameter("page", page.ToString());
            request.AddQueryParameter("size", size.ToString());
            var response = client.Execute(request);
            var json = JObject.Parse(response.Content);
            var res = json["success"].ToObject<bool>();
            if (res)
            {
                JToken data = json["data"];
                if (data != null)
                {
                    users = JsonConvert.DeserializeObject<List<User>>(data.ToString());
                    total_pages = json["total_pages"].ToObject<int>();
                }
            } else
            {
                string e = json["errors"].ToString();
                List<ServerError>? errors = JsonConvert.DeserializeObject<List<ServerError>>(e);
                throw new ServerException($"{getErrMsg(errors)}");
            }
            return (users, total_pages);
        }

        #endregion

    }
}
