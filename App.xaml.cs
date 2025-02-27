namespace TARpv23_Mobiile_App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new AppShell());
        }
    }
}
