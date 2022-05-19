using crm.Models.user;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.dialogs
{
    public class tagsDlgVM : BaseDialog
    {
        #region properties
        public override string Title => "Тэги";

        public ObservableCollection<string> Tags { get; }
        #endregion

        public tagsDlgVM(List<Role> roles)
        {
            //Tags = new ObservableCollection<string>()
            //{
            //    "Админ", "Кассир", "Комментарии", "Креативщик", "Медиа", "Тим-лид", "Связка", "Фарм"
            //};

            Tags = new ObservableCollection<string>();
            foreach (var item in roles)
                Tags.Add(item.ToString());

        }
    }
}
