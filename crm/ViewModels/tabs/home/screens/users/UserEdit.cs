using crm.Models.appcontext;
using crm.Models.user;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens.users
{
    public class UserEdit : BaseScreen
    {
        #region properties
        public ObservableCollection<BaseScreen> EditActions { get; }
        
        public override string Title => $"{AppContext.User.FirstName} {AppContext.User.LastName}";

        Object content;
        public Object Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }
        #endregion

        public UserEdit() : base(new ApplicationContext() { 
            User = new UserItemTest() })
        {
            EditActions = new ObservableCollection<BaseScreen>();
            EditActions.Add(new editUserInfo(AppContext));
            Content = EditActions[0];
        }
        public UserEdit(ApplicationContext context, BaseUser user) : base(context)
        {
            EditActions = new ObservableCollection<BaseScreen>();
            EditActions.Add(new editUserInfo(AppContext));
            Content = EditActions[0];
        }        
    }
}
