using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.api.socket
{
    public abstract class BaseSocketApi : ISocketApi
    {
        #region vars       
        Uri uri;
        SocketIO client;
        bool isConnected;

        public event Action<List<string>> ReceivedConnectedUsersEvent;
        #endregion

        public BaseSocketApi(string url)
        {
            uri = new Uri(url);
        }

        #region public
        public async Task Connect(string token)
        {
            client = new SocketIO(uri, new SocketIOOptions()
            {
                ExtraHeaders = new Dictionary<string, string>() {
                    { "Authorization", $"Bearer {token}" }
                }

            });
            client.OnConnected += Client_OnConnected;
            client.OnError += Client_OnError;
            client.OnDisconnected += Client_OnDisconnected;
            await client.ConnectAsync();            
        }

        private void Client_OnDisconnected(object? sender, string e)
        {
            isConnected = false;
        }
        private void Client_OnError(object? sender, string e)
        {
            isConnected = false;
        }

        private void Client_OnConnected(object? sender, EventArgs e)
        {
            isConnected = true;
            client.On("connected-users", (arg) => {
                Debug.WriteLine(arg);
            });
            client.On("", (arg) => { });
        }

        public void RequestConnectedUsers()
        {
            //if (!isConnected)
            //    throw new SocketApiException("Соединение с сервером не установлено (sock)");

            client.EmitAsync("get-connected-users");

        }
        #endregion
    }
}
