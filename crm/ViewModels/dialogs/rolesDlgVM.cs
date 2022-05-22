using Avalonia.Controls.Selection;
using crm.Models.appcontext;
using crm.Models.user;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.dialogs
{
    public class rolesDlgVM : BaseDialog
    {
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
        public ReactiveCommand<Unit, Unit> accpetCmd { get; }
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

            accpetCmd = ReactiveCommand.CreateFromTask(async () => {

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
