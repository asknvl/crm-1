using crm.Models.api.server;
using crm.Models.appcontext;
using crm.Models.user;
using crm.ViewModels.tabs.home.menu;
using crm.ViewModels.tabs.home.screens.users;
using crm.ViewModels.tabs.tabservice;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs
{
    public class homeVM : Tab
    {
        #region vars
        BaseServerApi api;
        ApplicationContext AppContext;
        #endregion

        #region properties     
        BaseMenu menu;
        public BaseMenu Menu
        {
            get => menu;
            set => this.RaiseAndSetIfChanged(ref menu, value);
        }
        //object screen;
        //public object Screen
        //{
        //    get => screen;
        //    set => this.RaiseAndSetIfChanged(ref screen, value);
        //}

        BaseUser user;
        BaseUser User
        {
            get => user;            
            set => this.RaiseAndSetIfChanged(ref user, value);                         
            
        }

        string initialLetter;
        public string InitialLetter
        {
            get => initialLetter;
            set => this.RaiseAndSetIfChanged(ref initialLetter, value);
        }

        bool isProfileMenuOpen;
        public bool IsProfileMenuOpen
        {
            get => isProfileMenuOpen;
            set {
                isProfileMenuOpen = value;
                this.RaisePropertyChanged("IsProfileMenuOpen");
            }
        }

        #endregion

        #region commands        
        public ReactiveCommand<Unit, Unit> profileMenuOpenCmd { get; }
        public ReactiveCommand<Unit, Unit> editUserCmd { get; }
        public ReactiveCommand<Unit, Unit> quitCmd { get; }
        #endregion

        public homeVM() : base()
        {
            User = new TestUser();
            Menu = new admin_menu();
        }

        public homeVM(ApplicationContext appcontext) : base()
        {


            #region commands
            profileMenuOpenCmd = ReactiveCommand.Create(() => {                        
                IsProfileMenuOpen = true;
            });

            editUserCmd = ReactiveCommand.Create(() =>
            {
                appcontext.TabService.ShowTab(new ScreenTab(new UserEdit(appcontext, appcontext.User)));
            });

            quitCmd = ReactiveCommand.Create(() =>
            {
                OnCloseTab();
            });
            #endregion

            User = appcontext.User;                        
            Title = "Домой";

            Menu = new admin_menu(appcontext);
            
        }
    }
}
