using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Thesis_SCADA.ViewModel
{
    public class MainViewModel: BaseViewModel
    {
        public MainViewModel()
        {       
            if (!GlobalVariable.Isloaded)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
                GlobalVariable.Isloaded = true;
            }
        }
    }

    public static class GlobalVariable
    {
        public static bool Isloaded = false;
    }
}
