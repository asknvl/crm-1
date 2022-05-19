using crm.Models.appcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens.users
{
    public class editUserInfo : BaseScreen
    {
        #region properties
        public override string Title => "Редактировать";
        #endregion
        public editUserInfo(ApplicationContext context) : base(context)
        {
        }
    }
}
