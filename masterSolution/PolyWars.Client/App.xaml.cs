using PolyWars.Network;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace PolyWars.Client {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        void App_Exit(object sender, ExitEventArgs e) {
            // send server a logout signal
            GameService gm = NetworkController.GameService;
            Debug.WriteLine("App Closed - Logging out client");
            if(NetworkController.IsConnected) {
                Task.Run(async () => await gm.LogoutAsync()).Wait();
            }
        }
    }
}
