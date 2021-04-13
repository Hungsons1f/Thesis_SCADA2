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
using Thesis_SCADA.Model;
using Thesis_SCADA.ViewModel;
using Thesis_SCADA.ViewModel.UserControlVM;

namespace Thesis_SCADA.UserControls
{
    /// <summary>
    /// Interaction logic for ucPopupFan.xaml
    /// </summary>
    //public partial class ucPopupFan : UserControl
    //{
    //    public ucPopupFan()
    //    {
    //        InitializeComponent();
    //    }
    //}

    public partial class ucPopupFan : UserControl
    {
        public ucPopupFanViewModel ViewModel { get; set; }
        public ucPopupFan()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ucPopupFanViewModel();
        }

        #region Dependency Properties
        public aFb_Motor MotorObject
        {
            get { return (aFb_Motor)GetValue(MotorObjectProperty); }
            set { SetValue(MotorObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MotorObjectProperty =
            DependencyProperty.Register("MotorObject", typeof(aFb_Motor), typeof(ucPopupFan), new FrameworkPropertyMetadata(null));

        public string Prefix
        {
            get { return (string)GetValue(PrefixProperty); }
            set { SetValue(PrefixProperty, value); }
        }

        // Using a DependencyProperty as the backing store for navframe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PrefixProperty =
            DependencyProperty.Register("Prefix", typeof(string), typeof(ucPopupFan), new FrameworkPropertyMetadata(null));

        #endregion

        #region Moving Popup
        double FirstXPos, FirstYPos, FirstArrowXPos, FirstArrowYPos;
        object MovingObject;
        private Canvas canvas;

        private void PopupFan_Loaded(object sender, RoutedEventArgs e)
        {
            canvas = Parent as Canvas;
            if (canvas == null)
            {
                throw new InvalidCastException("The parent of a KeyboardPopup control must be a Canvas.");
            }
            canvas.PreviewMouseMove -= PopupFan_MouseMove;
            canvas.PreviewMouseMove += PopupFan_MouseMove;
        }

        private void PopupFan_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FirstXPos = e.GetPosition(sender as Control).X;
            FirstYPos = e.GetPosition(sender as Control).Y;
            FirstArrowXPos = e.GetPosition((sender as Control).Parent as Control).X - FirstXPos;
            FirstArrowYPos = e.GetPosition((sender as Control).Parent as Control).Y - FirstYPos;
            MovingObject = sender;
        }

        void PopupFan_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MovingObject = null;
        }

        private void PopupFan_MouseMove(object sender, MouseEventArgs e)
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
