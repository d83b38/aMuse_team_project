using System.Windows;

namespace aMuse
{
    public partial class App : Application
    {
        public App()
        {
            Startup += App_Startup;
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            UI.MainWindow.GetInstance().Show();
        }
    }
}
