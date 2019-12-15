using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PolyWars.Client.Model {
    /// <summary>
    /// Base class for making a Class able to observe changes
    /// </summary>
    public class Observable : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
