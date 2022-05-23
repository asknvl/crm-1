using Avalonia.Controls.Selection;
using Avalonia.Data;
using crm.Models.appcontext;
using crm.Models.autocompletions;
using crm.Models.user;
using crm.Models.validators;
using crm.ViewModels.dialogs;
using crm.ViewModels.Helpers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
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
        IValidator<string> phone_vl = new PhoneNumberValidator();
        IValidator<string> birth_vl = new BirthDateValidator();
        IValidator<string> tg_vl = new TelegramValidator();
        IValidator<string> wallet_vl = new WalletValidator();

        bool
           isEmail,
           isFullName,
           isBirthDate,
           isPhoneNumber,
           isTelegram,
           isWallet;

        TagsAndRolesConvetrer convetrer = new();
        public List<tagsListItem> SelectedTags { get; } = new();
        public SelectionModel<tagsListItem> Selection { get; }
        #endregion

        #region properties
        public override string Title => "Редактировать";

        bool isInputValid;
        public bool IsInputValid
        {
            get => isInputValid;
            set => this.RaiseAndSetIfChanged(ref isInputValid, value);
        }

        string fullname;
        public string FullName
        {
            get => fullname;
            set
            {
                isFullName = fn_vl.IsValid(value);
                if (!isFullName)
                    throw new DataValidationException(fn_vl.Message);
                updateValidity();
                this.RaiseAndSetIfChanged(ref fullname, value);
            }
        }

        string email;
        public string Email
        {
            get => email;
            set
            {
                isEmail = email_vl.IsValid(value);
                if (!isEmail)
                    throw new DataValidationException("Введен некорректный e-mail");
                updateValidity();
                this.RaiseAndSetIfChanged(ref email, value);
            }
        }

        string phonenumber;
        public string PhoneNumber
        {
            get => phonenumber;
            set
            {
                isPhoneNumber = phone_vl.IsValid(value);
                if (!isPhoneNumber)
                    throw new DataValidationException(phone_vl.Message);
                updateValidity();
                this.RaiseAndSetIfChanged(ref phonenumber, value);
            }
        }

        string birthdate;
        public string BirthDate
        {
            get => birthdate;
            set
            {
                isBirthDate = birth_vl.IsValid(value);
                if (!isBirthDate)
                    throw new DataValidationException(birth_vl.Message);
                updateValidity();
                this.RaiseAndSetIfChanged(ref birthdate, value);
            }
        }

        public ObservableCollection<SocialNetwork> SocialNetworks { get; } = new ObservableCollection<SocialNetwork>();

        string telegram;
        public string Telegram
        {
            get => telegram;
            set
            {
                isTelegram = tg_vl.IsValid(value);
                if (!isTelegram)
                    throw new DataValidationException(tg_vl.Message);
                updateValidity();
                this.RaiseAndSetIfChanged(ref telegram, value);
            }
        }

        string wallet;
        public string Wallet
        {
            get => wallet;
            set
            {
                isWallet = wallet_vl.IsValid(value);
                if (!isWallet)
                    throw new DataValidationException(wallet_vl.Message);
                updateValidity();
                this.RaiseAndSetIfChanged(ref wallet, value);
            }
        }

        public ObservableCollection<tagsListItem> Tags { get; } = new();

        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> editCmd { get; }
        public ReactiveCommand<Unit, Unit> saveCmd { get; }
        public ReactiveCommand<Unit, Unit> openTelegram { get; }
        #endregion

        public editUserInfo() : base(new ApplicationContext())
        {

            TestUser user = new TestUser();
            FullName = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            BirthDate = user.BirthDate;
            Telegram = user.Telegram;
            Wallet = user.Wallet;

            foreach (var item in user.SocialNetworks)
                SocialNetworks.Add(item);

            openTelegram = ReactiveCommand.Create(() => {
                Process.Start(new ProcessStartInfo
                {
                    FileName = $"tg://resolve?domain={Telegram}",
                    UseShellExecute = true
                });
            });

            Tags = convetrer.GetAllTags();

            editCmd = ReactiveCommand.Create(() => { });
            saveCmd = ReactiveCommand.Create(() => { });

        }

        public editUserInfo(ApplicationContext context) : base(context)
        {
            BaseUser user = context.User;

            FullName = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            BirthDate = user.BirthDate;
            Telegram = user.Telegram;
            Wallet = user.Wallet;

            foreach (var item in user.SocialNetworks)
                SocialNetworks.Add(item);

            editCmd = ReactiveCommand.Create(() => { });
            saveCmd = ReactiveCommand.Create(() => { });
        }

        #region helpers
        protected bool CheckValidity(bool[] fields)
        {
            return !fields.Any(p => p == false);
        }
        void updateValidity()
        {
            IsInputValid = CheckValidity(new bool[] {
                isEmail,
                isFullName,
                isBirthDate,
                isPhoneNumber,
                isTelegram,
                isWallet
            });
        }
        #endregion
    }
}
