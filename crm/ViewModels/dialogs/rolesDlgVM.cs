using Avalonia.Controls.Selection;
using crm.Models.api.server;
using crm.Models.appcontext;
using crm.Models.user;
using crm.WS;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using TextCopy;

namespace crm.ViewModels.dialogs
{
    public class rolesDlgVM : BaseDialog
    {
        #region vars
        IWindowService ws = WindowService.getInstance();
        #endregion

        #region properties
        bool isValidSelection = false;
        public bool IsValidSelection
        {
            get => isValidSelection;
            set => this.RaiseAndSetIfChanged(ref isValidSelection, value);
        }
        public ObservableCollection<tagsListItem> Tags { get; } = new();
        public List<tagsListItem> SelectedTags { get; } = new();
        public SelectionModel<tagsListItem> Selection { get; }       
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        public ReactiveCommand<Unit, Unit> acceptCmd { get; }
        #endregion

        public rolesDlgVM()
        {

            IsValidSelection = false;

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

        public rolesDlgVM(ApplicationContext appcontext)
        {
            IServerApi api = appcontext.ServerApi;
            string token = appcontext.User.Token;

            Selection = new SelectionModel<tagsListItem>();
            Selection.SingleSelect = false;
            Selection.SelectionChanged += Selection_SelectionChanged;
            

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

            #region commands    
            cancelCmd = ReactiveCommand.Create(() => {
                OnCloseRequest();
            });

            acceptCmd = ReactiveCommand.CreateFromTask(async () => {

                List<Role> roles = new List<Role>();

                bool isAdmin = SelectedTags.Any(t => t.Name.Equals(Role.admin));
                bool isTeamLead = SelectedTags.Any(t => t.Name.Equals(Role.teamlead));                
                bool isComment = SelectedTags.Any(t => t.Name.Equals(Role.comment));                
                bool isMedia = SelectedTags.Any(t => t.Name.Equals(Role.media));
                bool isLink = SelectedTags.Any(t => t.Name.Equals(Role.link));
                bool isFarm = SelectedTags.Any(t => t.Name.Equals(Role.farm));
                bool isCreative = SelectedTags.Any(t => t.Name.Equals(Role.creative));
                bool isFinancier = SelectedTags.Any(t => t.Name.Equals(Role.financier));

                if (isAdmin)
                    roles.Add(new Role(RoleType.admin));

                if (isTeamLead)
                {                    
                    if (isComment)
                        roles.Add(new Role(RoleType.team_lead_comment));
                    if (isMedia)
                        roles.Add(new Role(RoleType.team_lead_media));
                    if (isLink)
                        roles.Add(new Role(RoleType.team_lead_link));
                    if (isFarm)
                        roles.Add(new Role(RoleType.team_lead_farm));
                } else
                {
                    if (isComment)
                        roles.Add(new Role(RoleType.buyer_comment));
                    if (isMedia)
                        roles.Add(new Role(RoleType.buyer_media));
                    if (isLink)
                        roles.Add(new Role(RoleType.buyer_link));
                    if (isFarm)
                        roles.Add(new Role(RoleType.buyer_farm));
                }

                if (isFinancier)
                    roles.Add(new Role(RoleType.financier));
                if (isCreative)
                    roles.Add(new Role(RoleType.creative));

                //Debug.WriteLine("----------");
                //foreach (var item in roles)
                //    Debug.WriteLine(item.Name);

                string newtoken = "";
                try
                {
                    newtoken = await api.GetNewUserToken(roles, token);

                    OnCloseRequest();

                } catch (Exception ex)
                {
                    ws.ShowDialog(new errMsgVM(ex.Message));
                }

                Clipboard clipboard = new Clipboard();
                try
                {
                    clipboard.SetText(newtoken);
                } catch { }

            });
            #endregion
        }

        private void Selection_SelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<tagsListItem> e)
        {

            foreach (var item in e.SelectedItems)
            {
                SelectedTags.Add(item);
            }

            foreach (var item in e.DeselectedItems)
            {                
                SelectedTags.Remove(item);
            }

            bool isTeamLead = SelectedTags.Any( t => t.Name.Equals(Role.teamlead));
            bool isAdmin = SelectedTags.Any(t => t.Name.Equals(Role.admin));
            bool isAnyOne = SelectedTags.Any(t => !t.Name.Equals(Role.teamlead) && !t.Name.Equals(Role.admin));

            IsValidSelection = (isAdmin && !isTeamLead ) || (isTeamLead && isAnyOne) || isAnyOne;

        }
    }
}
