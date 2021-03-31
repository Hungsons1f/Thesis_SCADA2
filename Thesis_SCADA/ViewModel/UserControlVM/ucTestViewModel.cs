using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Thesis_SCADA.UserControls;

namespace Thesis_SCADA.ViewModel
{
    public class ucTestViewModel: BaseViewModel
    {
        #region Commands
        public ICommand Button1Command { get; set; }
        public ICommand Button2Command { get; set; }

        #endregion

        public ucTestViewModel()
        {
            Button1Command = new RelayCommand<ucTest>((p) => { return p == null ? false : true; }, (p) => {
                Frame Region = p.NavFrame;
                Region.Navigate(new System.Uri("Pages/TestPage.xaml", UriKind.RelativeOrAbsolute));
            });

            Button2Command = new RelayCommand<ucTest>((p) => { return p == null ? false : true; }, (p) => {
                Frame Region = p.NavFrame;
                Region.Navigate(new System.Uri("Pages/TestPage2.xaml", UriKind.RelativeOrAbsolute));
            });
        }
    }
}
