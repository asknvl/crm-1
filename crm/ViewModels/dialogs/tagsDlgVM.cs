using crm.Models.user;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.dialogs
{
    public class tagsListItem : ViewModelBase
    {
        string name;
        public string Name {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);  
        }

        bool ischecked;
        public bool IsChecked
        {
            get => ischecked;
            set => this.RaiseAndSetIfChanged(ref ischecked, value);
        }

        public tagsListItem(string name)
        {
            Name = name;
            IsChecked = false;
        }
    }
    public class tagsDlgVM : BaseDialog
    {
        #region const
        const string admin = "Админ";
        const string financier = "Кассир";
        const string comment = "Комментарии";
        const string creative = "Креативщик";
        const string media = "Медиа";
        const string teamlead = "Тим-лид";
        const string link = "Связка";
        const string farm = "Фарм";
        #endregion

        #region properties
        public override string Title => "Тэги";
        public ObservableCollection<tagsListItem> Tags { get; } = new ObservableCollection<tagsListItem>();
        public List<tagsListItem> SelectedTags { get; } = new();
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> closeCmd { get; }
        #endregion

        public tagsDlgVM()
        {
            #region commands
            closeCmd = ReactiveCommand.Create(() => {
                OnCloseRequest();
            });
            #endregion
            Tags = new ObservableCollection<tagsListItem>() {
                new tagsListItem(admin),
                new tagsListItem(financier),
                new tagsListItem(comment),
                new tagsListItem(creative),
                new tagsListItem(media),
                new tagsListItem(teamlead),
                new tagsListItem(link),
                new tagsListItem(farm),
                new tagsListItem(creative)
            };

        }

        public tagsDlgVM(List<Role> roles)
        {
            closeCmd = ReactiveCommand.Create(() => {
                OnCloseRequest();
            });

            var adminItem = new tagsListItem(admin);
            var financierItem = new tagsListItem(financier);
            var commentItem = new tagsListItem(comment);
            var creativeItem = new tagsListItem(creative);
            var mediaItem = new tagsListItem(media);
            var teamleadItem = new tagsListItem(teamlead);
            var linkItem = new tagsListItem(link);
            var farmItem = new tagsListItem(farm);
            var tagsItem = new tagsListItem(creative);

            Tags = new ObservableCollection<tagsListItem>() {
                adminItem,
                financierItem,
                commentItem,
                creativeItem,
                mediaItem,
                teamleadItem,
                linkItem,
                farmItem
            };

            bool adm = roles.Any(r => r.Type == RoleType.admin);
            if (adm)
                SelectedTags.Add(adminItem);
            

            bool tl = roles.Any(r =>
                r.Type == RoleType.team_lead_comment ||
                r.Type == RoleType.team_lead_farm ||
                r.Type == RoleType.team_lead_link ||
                r.Type == RoleType.team_lead_media
            );
            if (tl)
                SelectedTags.Add(teamleadItem);

            bool fin = roles.Any(r => r.Type == RoleType.financier);
            if (fin)
                SelectedTags.Add(financierItem);

            bool com = roles.Any( r =>
                r.Type == RoleType.team_lead_comment ||
                r.Type == RoleType.buyer_comment
            );
            if (com)
                SelectedTags.Add(commentItem);

            bool frm = roles.Any(r =>
               r.Type == RoleType.team_lead_farm ||
               r.Type == RoleType.buyer_farm
            );
            if (frm)
                SelectedTags.Add(farmItem);

            bool lnk = roles.Any(r =>
               r.Type == RoleType.team_lead_link ||
               r.Type == RoleType.buyer_link
            );
            if (lnk)
                SelectedTags.Add(linkItem);

            bool med = roles.Any(r =>
               r.Type == RoleType.team_lead_media ||
               r.Type == RoleType.buyer_media
            );
            if (med)
                SelectedTags.Add(mediaItem);

            bool cre = roles.Any(r =>
               r.Type == RoleType.creative
            );
            if (cre)
                SelectedTags.Add(creativeItem);

        }
    }
}
