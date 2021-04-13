using System;
using System.Collections.Generic;
using System.Globalization;
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
using Thesis_SCADA.Model;
using Thesis_SCADA.ViewModel;
using Thesis_SCADA.ViewModel.UserControlVM;

namespace Thesis_SCADA.UserControls
{
    /// <summary>
    /// Interaction logic for ucPopupValve.xaml
    /// </summary>
    public partial class ucPopupValve : UserControl
    {
        public ucPopupValveViewModel ViewModel { get; set; }
        public ucPopupValve()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ucPopupValveViewModel();
        }

        #region Dependency Properties
        public aFb_Valve ValveObject
        {
            get { return (aFb_Valve)GetValue(ValveObjectProperty); }
            set { SetValue(ValveObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValveObjectProperty =
            DependencyProperty.Register("ValveObject", typeof(aFb_Valve), typeof(ucPopupValve), new FrameworkPropertyMetadata(null));

        public string Prefix
        {
            get { return (string)GetValue(PrefixProperty); }
            set { SetValue(PrefixProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PrefixProperty =
            DependencyProperty.Register("Prefix", typeof(string), typeof(ucPopupValve), new FrameworkPropertyMetadata(null));

        #endregion

        #region Moving Popup
        double FirstXPos, FirstYPos, FirstArrowXPos, FirstArrowYPos;
        object MovingObject;
        private Canvas canvas;

        private void PopupValve_Loaded(object sender, RoutedEventArgs e)
        {
            canvas = Parent as Canvas;
            if (canvas == null)
            {
                throw new InvalidCastException("The parent of a KeyboardPopup control must be a Canvas.");
            }
            canvas.PreviewMouseMove -= PopupValve_MouseMove;
            canvas.PreviewMouseMove += PopupValve_MouseMove;
        }

        private void PopupValve_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FirstXPos = e.GetPosition(sender as Control).X;
            FirstYPos = e.GetPosition(sender as Control).Y;
            FirstArrowXPos = e.GetPosition((sender as Control).Parent as Control).X - FirstXPos;
            FirstArrowYPos = e.GetPosition((sender as Control).Parent as Control).Y - FirstYPos;
            MovingObject = sender;
        }

        void PopupValve_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MovingObject = null;
        }

        private void PopupValve_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (MovingObject != null)
                {
                    (MovingObject as FrameworkElement).SetValue(Canvas.LeftProperty,
                        e.GetPosition((MovingObject as FrameworkElement).Parent as FrameworkElement).X - FirstXPos);

                    (MovingObject as FrameworkElement).SetValue(Canvas.TopProperty,
                         e.GetPosition((MovingObject as FrameworkElement).Parent as FrameworkElement).Y - FirstYPos);

                }
            }
        }
        #endregion
    }
}
