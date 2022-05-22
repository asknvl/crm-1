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
    public class tagsDlgVM : BaseDialog
    {
        #region properties        
        public ObservableCollection<tagsListItem> Tags { get; } = new ObservableCollection<tagsListItem>();
        public List<tagsListItem> SelectedTags { get; } = new();
        #endregion

        public tagsDlgVM()
        {        
            Tags = new ObservableCollection<tagsListItem>() {
                new tagsListItem(Role.admin),
                new tagsListItem(Role.financier),
                new tagsListItem(Role.comment),
                new tagsListItem(Role.creative),
                new tagsListItem(Role.media),
                new tagsListItem(Role.teamlead),
                new tagsListItem(Role.link),
                new tagsListItem(Role.farm),
                new tagsListItem(Role.creative)
            };
        }

        public tagsDlgVM(List<Role> roles)
        {           

            var adminItem = new tagsListItem(Role.admin);
            var financierItem = new tagsListItem(Role.financier);
            var commentItem = new tagsListItem(Role.comment);
            var creativeItem = new tagsListItem(Role.creative);
            var mediaItem = new tagsListItem(Role.media);
            var teamleadItem = new tagsListItem(Role.teamlead);
            var linkItem = new tagsListItem(Role.link);
            var farmItem = new tagsListItem(Role.farm);
            var tagsItem = new tagsListItem(Role.creative);

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
