using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Thesis_SCADA.Model;

namespace Thesis_SCADA.ViewModel
{
    public class SettingViewModel : BaseViewModel
    {
        #region Properties
        private bool isDarkTheme;
        public bool IsDarkTheme
        {
            get => isDarkTheme;
            set
            {
                isDarkTheme = value;
                OnPropertyChanged();

                var paletteHelper = new PaletteHelper();
                var theme = paletteHelper.GetTheme();
                theme.SetBaseTheme(isDarkTheme ? Theme.Dark : Theme.Light);
                paletteHelper.SetTheme(theme);
            }
        }
        #endregion

        #region Commands
        public ICommand AddCommand { get; set; }
        #endregion

        public SettingViewModel()
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark;

            AddCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
            });

        }
    }

}
