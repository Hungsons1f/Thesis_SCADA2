using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thesis_SCADA.ViewModel;

namespace Thesis_SCADA.UserControls
{
    /// <summary>
    /// Interaction logic for ucPopupPump.xaml
    /// </summary>
    public partial class ucPopupPump : UserControl
    {
        double FirstXPos, FirstYPos, FirstArrowXPos, FirstArrowYPos;
        object MovingObject;
        private Canvas canvas;

        public ucPopupPumpViewModel ViewModel { get; set; }
        public ucPopupPump()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ucPopupPumpViewModel();
        }

        private void PopupPump_Loaded(object sender, RoutedEventArgs e)
        {
            canvas = Parent as Canvas;
            if (canvas == null)
            {
                throw new InvalidCastException("The parent of a KeyboardPopup control must be a Canvas.");
            }
            canvas.PreviewMouseMove -= PopupPump_MouseMove;
            canvas.PreviewMouseMove += PopupPump_MouseMove;
        }

        private void PopupPump_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FirstXPos = e.GetPosition(sender as Control).X;
            FirstYPos = e.GetPosition(sender as Control).Y;
            FirstArrowXPos = e.GetPosition((sender as Control).Parent as Control).X - FirstXPos;
            FirstArrowYPos = e.GetPosition((sender as Control).Parent as Control).Y - FirstYPos;
            MovingObject = sender;
        }

        void PopupPump_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MovingObject = null;
        }

        private void PopupPump_MouseMove(object sender, MouseEventArgs e)
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
    }
}
