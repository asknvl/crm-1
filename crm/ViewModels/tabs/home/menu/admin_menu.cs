using crm.Models.appcontext;
using crm.Models.user;

namespace crm.ViewModels.tabs.home.menu
{
    public class admin_menu : BaseMenu
    {

        public admin_menu() : base(new ApplicationContext())
        {
            ApplicationContext context = new ApplicationContext();
            context.ServerApi = new Models.api.server.ServerApi("");
            context.User = new TestUser();
            
            SimpleMenuItem dashboard = new items.Dashboard();
            dashboard.AddScreen(new screens.Dashboard(context));
            AddItem(dashboard);

            ComplexMenuItem users = new items.Users();
            users.AddScreen(new screens.UserList(context));
            users.AddScreen(new screens.UserActions(context));
            AddItem(users);
        }

        public admin_menu(ApplicationContext context) : base(context)
        {

            SimpleMenuItem dashboard = new items.Dashboard();
            dashboard.AddScreen(new screens.Dashboard(context));                        
            AddItem(dashboard);

            ComplexMenuItem users = new items.Users();            
            users.AddScreen(new screens.UserList(context));
            users.AddScreen(new screens.UserActions(context));            
            AddItem(users);

            SimpleMenuItem accimport = new items.AccountsImport();
            accimport.AddScreen(new screens.AccountsImport(context));
            AddItem(accimport);

            SimpleMenuItem creatives = new items.Creatives();
            creatives.AddScreen(new screens.Creatives(context));
            AddItem(creatives);

            SimpleMenuItem subscriptions = new items.Subscriptions();
            subscriptions.AddScreen(new screens.Subscriptions(context));
            AddItem(subscriptions);

            ComplexMenuItem proxies = new items.Proxies();
            proxies.AddScreen(new screens.TBD(context));
            proxies.AddScreen(new screens.TBD(context));
            AddItem(proxies);

            SimpleMenuItem devices = new items.Devices();
            devices.AddScreen(new screens.Devices(context));
            AddItem(devices);

            SimpleMenuItem geo = new items.GEO();
            geo.AddScreen(new screens.GEO(context));
            AddItem(geo);

            ComplexMenuItem finances = new items.Finances();
            finances.AddScreen(new screens.Bills(context));
            finances.AddScreen(new screens.Expenses(context));
            AddItem(finances);

            ComplexMenuItem accounts = new items.Accounts();
            accounts.AddScreen(new screens.TBD(context));
            accounts.AddScreen(new screens.TBD(context));
            AddItem(accounts);
        }
        
    }
}
