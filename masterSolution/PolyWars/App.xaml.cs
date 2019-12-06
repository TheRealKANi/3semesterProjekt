using PolyWars.Network;
using System.Diagnostics;
using System.Windows;

namespace PolyWars {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        void App_Exit(object sender, ExitEventArgs e) {
            // send server a logout signal
            GameService gm = NetworkController.GameService;
            Debug.WriteLine("App Closed - Logging out");
            gm.LogoutAsync();
        }

    }
}
