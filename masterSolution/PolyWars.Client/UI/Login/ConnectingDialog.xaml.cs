using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace PolyWars.Client.UI.Login {
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
