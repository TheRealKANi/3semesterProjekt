using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace PolyWars.UI.Login {
    /// <summary>
    /// Interaction logic for ConnectingDialog.xaml
    /// </summary>
    public partial class ConnectingDialog : UserControl, INotifyPropertyChanged {
        public ConnectingDialog() {
            InitializeComponent();
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ConnectingDialog), new PropertyMetadata(""));
        public string Text {
            get { 
                return (string) GetValue(TextProperty) ?? ""; 
            }
            set { 
                SetValue(TextProperty, value);
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
