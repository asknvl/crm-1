using crm.Models.user;
using crm.ViewModels.dialogs;
using crm.WS;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens.users
{
    public class UserListItem : BaseUser
    {
        #region vars
        IWindowService ws = WindowService.getInstance();
        #endregion

        #region properties
        bool status;
        public bool Status
        {
            get => status;
            set => this.RaiseAndSetIfChanged(ref status, value);
        }

        //public string ShortWallet => $"{Wallet.Substring(0, 15)}...";
        public string ShortWallet => $"{Wallet}";
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> showTagsCmd { get; }
        #endregion

        public UserListItem()
        {
            #region commands
            showTagsCmd = ReactiveCommand.Create(() => {
                tagsDlgVM tags = new tagsDlgVM(Roles);
                ws.ShowDialog(tags);
            });
            #endregion
        }
    }
}
