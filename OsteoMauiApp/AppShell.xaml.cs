using OsteoMAUIApp.Views.Home;

namespace OsteoMAUIApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("Dashboard", typeof(Dashboard));

        }
    }
}
