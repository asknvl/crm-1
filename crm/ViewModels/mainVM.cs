using Avalonia.Controls;
using crm.Models.api.server;
using crm.Models.api.socket;
using crm.Models.appcontext;
using crm.Models.user;
using crm.ViewModels.tabs;
using crm.ViewModels.tabs.home.screens.users;
using crm.ViewModels.tabs.tabservice;
using ReactiveUI;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace crm.ViewModels
{
    public class mainVM : ViewModelBase, ITabService
    {

        #region vars
        homeVM homeTab;
        loginVM loginTab;
        tokenVM tokenTab;
        registrationVM registrationTab;
        //BaseServerApi api;
        #endregion

        #region properties      
        public ObservableCollection<Tab> TabsList { get; set; } = new ObservableCollection<Tab>();

        object? content;
        public object? Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }

        WindowState windowState;
        public WindowState WindowState { 
            get => windowState; 
            set => this.RaiseAndSetIfChanged(ref windowState, value); 
        }

        BaseUser user;
        BaseUser User
        {
            get => user;
            set => this.RaiseAndSetIfChanged(ref user, value);

        }

        bool isUserMenuVisible;
        public bool IsUserMenuVisible
        {
            get => isUserMenuVisible;
            set => this.RaiseAndSetIfChanged(ref isUserMenuVisible, value);
        }

        bool isProfileMenuOpen;
        public bool IsProfileMenuOpen
        {
            get => isProfileMenuOpen;
            set
            {
                isProfileMenuOpen = value;
                this.RaisePropertyChanged("IsProfileMenuOpen");
            }
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> closeCmd { get; }
        public ReactiveCommand<Unit, Unit> maximizeCmd { get; }
        public ReactiveCommand<Unit, Unit> minimizeCmd { get; }        
        public ReactiveCommand<Unit, Unit> profileMenuOpenCmd { get; }
        public ReactiveCommand<Unit, Unit> editUserCmd { get; }
        public ReactiveCommand<Unit, Unit> quitCmd { get; }
        #endregion
        public mainVM()
        {

            #region dependencies
            ApplicationContext AppContext = new ApplicationContext();
            AppContext.ServerApi = new ServerApi("http://185.46.9.229:4000");
            AppContext.SocketApi = new SocketApi("http://185.46.9.229:4000");
            
            AppContext.TabService = this;            
            #endregion

            #region init
            WindowState = WindowState.Normal;
            #endregion

            #region commands           
            maximizeCmd = ReactiveCommand.Create(() => {
                if (WindowState == WindowState.Maximized)
                    WindowState = WindowState.Normal;
                else
                if (WindowState == WindowState.Normal)
                    WindowState = WindowState.Maximized;
                else
                if (WindowState == WindowState.Minimized)
                    WindowState = WindowState.Maximized;
            });

            closeCmd = ReactiveCommand.Create(() => {
                OnCloseRequest();
            });

            minimizeCmd = ReactiveCommand.Create(() => {
                WindowState = WindowState.Normal;
                WindowState = WindowState.Minimized;
            });

            profileMenuOpenCmd = ReactiveCommand.Create(() => {
                IsProfileMenuOpen = true;
            });

            editUserCmd = ReactiveCommand.Create(() =>
            {
                AppContext.TabService.ShowTab(new ScreenTab(new UserEdit(AppContext, AppContext.User)));
            });

            quitCmd = ReactiveCommand.Create(() =>
            {
                //разлогин для юзера OnDeactivate для всех экранов
            });
            #endregion

            #region registrationTab
            registrationTab = new registrationVM(AppContext);
            registrationTab.onUserRegistered += () => 
            {
                CloseTab(registrationTab);
                loginTab.Clear();
                ShowTab(loginTab);
            };
            //registrationTab.CloseTabEvent += CloseTab;
            #endregion

            #region loginTab
            loginTab = new loginVM(AppContext);            
            loginTab.onLoginDone += async (user) => {

                User = user;
                IsUserMenuVisible = true;

                AppContext.User = user;
                
                await AppContext.SocketApi.Connect(user.Token);
                //AppContext.SocketApi.RequestConnectedUsers();
                
                homeVM homeTab = new homeVM(AppContext);                
                homeTab.CloseTabEvent += (tab) =>
                {
                    CloseTab(tab);
                    loginTab.Password = "";
                    ShowTab(loginTab);
                };
                CloseTab(loginTab);                
                ShowTab(homeTab);
            };
            loginTab.onCreateUserAction += () =>
            {
                ShowTab(tokenTab);
            };
            #endregion

            #region tokenTab
            tokenTab = new tokenVM(AppContext);
            //tokenTab.CloseTabEvent += CloseTab;
            tokenTab.onTokenCheckResult += (result, token) =>
            {

                if (result)
                {
                    CloseTab(tokenTab);
                    registrationTab.Token = token;
                    ShowTab(registrationTab);                    
                }
            };
            #endregion
                        
            ShowTab(loginTab);            
        }

        #region helpers
        public void ShowTab(Tab tab)
        {
            var fTab = TabsList.FirstOrDefault(t => t.Title.Equals(tab.Title));
            //if (fTab is homeVM)
            //{
            //    var found = TabsList.FirstOrDefault(o => o is homeVM);
            //    if (found != null)
            //        TabsList.Remove(found);
            //    Content = fTab;
            //    return;
            //}

            if (fTab == null)
            {

                tab.CloseTabEvent += CloseTab;

                if (tab is homeVM)
                {              

                    TabsList.Insert(0, tab);
                } else
                    TabsList.Add(tab);
                
                Content = tab;
            }
            else            
                Content = fTab;
            
        }

        public void AddTab(Tab tab)
        {
            var fTab = TabsList.FirstOrDefault(t => t.Title.Equals(tab.Title));
            if (fTab == null)
            {
                tab.CloseTabEvent += CloseTab;

                TabsList.Add(tab);                
            }            
        }

        public void CloseTab(Tab tab)
        {
            int index = TabsList.IndexOf(tab);
            if (index >= 1)
            {
                var prev = TabsList[index - 1];
                if (prev != null)
                    ShowTab(prev);
            }
            TabsList.Remove(tab);
        }       
        #endregion

    }
}
