using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PolyWars.UI.GameArena
{
    /// <summary>
    /// Interaction logic for GameArena.xaml
    /// </summary>
    public partial class GameArenaPage : Page
    {
        public GameArenaPage() {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            Keyboard.Focus(this);
            // KeyDown += (Resources["vm"] as GameArena_ViewModel).GameController.InputController.Input.onKeyStateChanged;
            // KeyUp += (Resources["vm"] as GameArena_ViewModel).GameController.InputController.Input.onKeyStateChanged;
            KeyDown += test;
        }

        private void test(object sender, KeyEventArgs e) {
            throw new NotImplementedException();
        }
    }
}
