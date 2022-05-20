using Avalonia.Data;
using crm.Models.appcontext;
using crm.Models.autocompletions;
using crm.Models.validators;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens.users
{
    public class editUserInfo : BaseScreen
    {
        #region vars
        IValidator<string> fn_vl = new FullNameValidator();
        IAutoComplete email_ac = new EmailAutoComplete();
        IValidator<string> email_vl = new LoginValidator();
        #endregion

        #region properties
        public override string Title => "Редактировать";

        string fullname;
        public string FullName
        {
            get => fullname;
            set
            {                
                //if (!fn_vl.IsValid(value))
                //    throw new DataValidationException(fn_vl.Message);                
                this.RaiseAndSetIfChanged(ref fullname, value);
            }
        }

        string email;
        public string Email
        {
            get => email;
            set
            {
                //value = email_ac.AutoComplete(value);
                //if (email_vl.IsValid(value))
                //    throw new DataValidationException("Введен некорректный e-mail");                
                this.RaiseAndSetIfChanged(ref email, value);
            }
        }

        #endregion
        public editUserInfo(ApplicationContext context) : base(context)
        {
            FullName = AppContext.User.FullName;    
            Email = AppContext.User.Email;
        }
    }
}
