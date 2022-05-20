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
        string initialLetter;
        public string InitialLetter
        {
            get => initialLetter;
            set => this.RaiseAndSetIfChanged(ref initialLetter, value);
        }
        #endregion

        #region commands        
        public ReactiveCommand<Unit, Unit> profileMenuOpenCmd { get; }
        public ReactiveCommand<Unit, Unit> editUserCmd { get; }
        public ReactiveCommand<Unit, Unit> quitCmd { get; }
        #endregion

        public homeVM() : base()
        {            
            Menu = new admin_menu();
        }

        public homeVM(ApplicationContext appcontext) : base()
       {

            Title = "Домой";

            Menu = new admin_menu(appcontext);
            
        }
    }
}
