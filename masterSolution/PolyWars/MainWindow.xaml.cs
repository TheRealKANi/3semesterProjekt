using PolyWars.Logic;
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

namespace PolyWars {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Input input;
        public MainWindow() {
            InitializeComponent();
            input = new Input();
            
        }


        //TODO Needs proper implementation
        private void Window_KeyUp(object sender, KeyEventArgs e) {
            input.onKeyStateChanged(sender, e);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            input.onKeyStateChanged(sender, e);
        }
    }
}
