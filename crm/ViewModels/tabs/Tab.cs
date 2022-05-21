using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;

namespace crm.ViewModels.tabs
{
    public abstract class Tab : ViewModelBase
    {
        #region properties       
        string title;
        public string Title
        {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
        }

        bool isInputValid;
        protected bool IsInputValid
        {
            get => isInputValid;
            set => this.RaiseAndSetIfChanged(ref isInputValid, value);
        }
        protected bool needValidate { get; set; }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> closeCmd { get; }
        #endregion

        public Tab()
        {           
            closeCmd = ReactiveCommand.Create(() => {
                CloseTabRequest?.Invoke(this);
            });
        }

        #region protected
        protected bool CheckValidity(bool[] fields)
        {
            return !fields.Any(p => p == false);
        }
        protected virtual void CloseRequest()
        {
            CloseTabRequest?.Invoke(this);
        }
        #endregion

        #region public                
        public virtual void Show()
        {
            ShowTabRequest?.Invoke(this);
        }

        public virtual void Close()
        {
            CloseTabRequest?.Invoke(this);
        }
        #endregion

        #region abstract        
        public virtual void Clear() { }
        #endregion

        #region callbacks
        public event Action<Tab> CloseTabRequest;
        public event Action<Tab> ShowTabRequest;
        #endregion
    }
}
