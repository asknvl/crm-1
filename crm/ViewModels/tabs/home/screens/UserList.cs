using Avalonia.Threading;
using crm.Models.api.server;
using crm.Models.api.socket;
using crm.Models.appcontext;
using crm.Models.user;
using crm.ViewModels.dialogs;
using crm.ViewModels.tabs.home.screens.users;
using crm.WS;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens
{
    public class UserList : BaseScreen
    {
        #region const        
        const int displayed_lines_num = 20;
        #endregion

        #region vars        
        IWindowService ws = WindowService.getInstance();
        IServerApi srvApi;
        ISocketApi sckApi;
        string token;
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> addUserCmd { get; }
        #endregion

        #region properties       
        public override string Title => "Список сотрудников";

        ObservableCollection<UserListItem> users = new();
        public ObservableCollection<UserListItem> Users
        {
            get => users;
            set => this.RaiseAndSetIfChanged(ref users, value);
        }

        int page = 1;
        public int SelectedPage
        {
            get => page;
            set
            {
                IsPrevActive = (value > 1);
                IsNextActive = (value < TotalPages);
                this.RaiseAndSetIfChanged(ref page, value);
            }
        }

        int totalPages = 1;
        public int TotalPages
        {
            get => totalPages;
            set
            {
                IsPrevActive = (SelectedPage > 1);
                IsNextActive = (SelectedPage < value || TotalPages == 0);
                this.RaiseAndSetIfChanged(ref totalPages, value);
            }
        }

        int pageSize = displayed_lines_num;
        public int PageSize
        {
            get => pageSize;
            set => this.RaiseAndSetIfChanged(ref pageSize, value);
        }

        bool isNextActive = true;
        public bool IsNextActive
        {
            get => isNextActive;
            set => this.RaiseAndSetIfChanged(ref isNextActive, value);
        }

        bool isPrevActive = true;
        public bool IsPrevActive
        {
            get => isPrevActive;
            set => this.RaiseAndSetIfChanged(ref isPrevActive, value);
        }

        string pageInfo;
        public string PageInfo
        {
            get => pageInfo;
            set => this.RaiseAndSetIfChanged(ref pageInfo, value);
        }
        #endregion

        #region commands
        ReactiveCommand<Unit, Unit> editUserCmd { get; }
        ReactiveCommand<Unit, Unit> showTagsCmd { get; }
        ReactiveCommand<Unit, Unit> showPaspCmd { get; }
        ReactiveCommand<Unit, Unit> nextPageCmd { get; }
        ReactiveCommand<Unit, Unit> prevPageCmd { get; }

        #endregion

        public UserList(ApplicationContext context) : base(context)
        {

            srvApi = AppContext.ServerApi;
            token = AppContext.User.Token;
            sckApi = AppContext.SocketApi;
            SelectedPage = 1;

            #region commands
            addUserCmd = ReactiveCommand.Create(() =>
            {
                ws.ShowDialog(new rolesDlgVM(AppContext));
            });

            editUserCmd = ReactiveCommand.Create(() =>
            {
            });

            showTagsCmd = ReactiveCommand.Create(() =>
            {
            });

            showPaspCmd = ReactiveCommand.Create(() =>
            {
            });

            prevPageCmd = ReactiveCommand.CreateFromTask(async () =>
            {
                SelectedPage--;
                try
                {
                    await updatePageInfo(SelectedPage, PageSize);
                }
                catch (Exception ex)
                {
                    ws.ShowDialog(new errMsgVM(ex.Message));
                }
            });

            nextPageCmd = ReactiveCommand.CreateFromTask(async () =>
            {
                SelectedPage++;
                try
                {
                    await updatePageInfo(SelectedPage, PageSize);
                }
                catch (Exception ex)
                {
                    ws.ShowDialog(new errMsgVM(ex.Message));
                }
            });
            #endregion
        }

        #region helpers
        string getPageInfo(int page, int users_count, int total_users)
        {
            int p = (users_count < displayed_lines_num) ? (page - 1) * displayed_lines_num + users_count : page * displayed_lines_num;
            return $"{(page - 1) * displayed_lines_num + 1}-{p} из {total_users}";
        }

        async Task updatePageInfo(int page, int pagesize)
        {
#if OFFLINE
            Users.Add(new UserItemTest(AppContext) { FullName = "Иванов Иван Иванович" });
            Users.Add(new UserItemTest(AppContext) { FullName = "Петров Петр Петрович" });
#elif ONLINE
            await Task.Run(async () =>
            {
                List<User> users;
                int total_users;

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Users.Clear();
                });

                (users, TotalPages, total_users) = await srvApi.GetUsers(page - 1, pagesize, token);

                PageInfo = getPageInfo(page, users.Count, total_users);

                foreach (var user in users)
                {
                    var tmp = new UserListItem(AppContext);
                    tmp.Copy(user);

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        Users.Add(tmp);
                    });
                }

                sckApi.RequestConnectedUsers();
            });
#endif
        }
        #endregion

        #region override
        public override async void OnActivate()
        {
            base.OnActivate();

            sckApi.ReceivedConnectedUsersEvent += SckApi_ReceivedConnectedUsersEvent;

            try
            {
                await updatePageInfo(SelectedPage, PageSize);
            }
            catch (OperationCanceledException ex)
            {
            }
            catch (Exception ex)
            {
                ws.ShowDialog(new errMsgVM(ex.Message));
            }
        }

        public override void OnDeactivate()
        {
            sckApi.ReceivedConnectedUsersEvent -= SckApi_ReceivedConnectedUsersEvent;
            base.OnDeactivate();
        }
        #endregion

        #region callbacks
        private void SckApi_ReceivedConnectedUsersEvent(List<usersOnlineDTO> connectedUsers)
        {
            foreach (var connected in connectedUsers)
            {
                var user = Users.FirstOrDefault(u => u.Id.Equals(connected.user_id));
                if (user != null)
                    user.Status = connected.connected;
            }
        }
        #endregion
    }
}
