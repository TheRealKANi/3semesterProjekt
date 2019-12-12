using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PolyWars.Model {
    /// <summary>
    /// ????
    /// </summary>
    public class Observable : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
