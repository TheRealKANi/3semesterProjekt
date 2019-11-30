using PolyWars.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PolyWars.UI.Login {
    class Login_ViewModel : Observable {
        private string name;
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
                NotifyPropertyChanged();
            }
        }
        private SecureString password;
        public SecureString Password {
            get {
                return password;
            }
            set {
                password = value;
                NotifyPropertyChanged();
            }
        }
        private ICommand loginCommand;

        public ICommand LoginCommand {
            get {
                if(loginCommand == null) {
                    loginCommand = new RelayCommand((o) => {
                        return !string.IsNullOrWhiteSpace(Name) ;
                    }, Login);
                }
                return loginCommand; 
            }
        }
        
        private void Login(object o) {
            if(o is string sstr) {
                //IntPtr stringPointer = Marshal.SecureStringToBSTR(sstr);
                //string normalString = Marshal.PtrToStringBSTR(stringPointer);
                //Marshal.ZeroFreeBSTR(stringPointer);
                //Debug.WriteLine(normalString);
            }
        }
    }
}
