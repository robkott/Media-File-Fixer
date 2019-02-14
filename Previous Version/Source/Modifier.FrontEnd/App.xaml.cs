using Common;
using Microsoft.Practices.Unity;

namespace Modifier.FrontEnd
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootStrapper = new BootStrapper();
            bootStrapper.Run();

            AddFrontEndEntities(bootStrapper.Container);

            var mainView = new MainView();
            mainView.Show();
        }

        private void AddFrontEndEntities(IUnityContainer container)
        {

        }
    }
}
