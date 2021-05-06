using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Thesis_SCADA.ViewModel
{
    public class ucControlBarViewModel: BaseViewModel
    {
        private bool ismaximize = true;
        #region commands 
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MouseLeftDownCommand { get; set; }
        #endregion

        public ucControlBarViewModel ()
        {
            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return p == null? false: true; }, (p) => {
                //Sau khi lấy được window, cần ép kiểu nó thành window và kiểm tra xem window có tồn tại hay không
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            });

            MaximizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
                //Sau khi lấy được window, cần ép kiểu nó thành window và kiểm tra xem window có tồn tại hay không
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    if (w.WindowState == WindowState.Normal)
                        w.WindowState = WindowState.Maximized;
                    else w.WindowState = WindowState.Normal;
                }
            });

            MinimizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
                //Sau khi lấy được window, cần ép kiểu nó thành window và kiểm tra xem window có tồn tại hay không
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    if (w.WindowState == WindowState.Normal)
                    {
                        w.WindowState = WindowState.Minimized;
                        ismaximize = false;
                    }
                    else if (w.WindowState == WindowState.Maximized)
                    {
                        w.WindowState = WindowState.Minimized;
                        ismaximize = true;
                    }
                    else
                    {
                        if (ismaximize)
                            w.WindowState = WindowState.Maximized;
                        else w.WindowState = WindowState.Normal;
                    }
                }
            });

            MouseLeftDownCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
                //Sau khi lấy được window, cần ép kiểu nó thành window và kiểm tra xem window có tồn tại hay không
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            });

        }

        //Để đóng một cửa sổ, ta cần phải truyền vào chính cửa sổ đó làm tham số cho command đóng. Tuy nhiên, trong 
        //usercontrol, ta không có window, mà Window chính là thẻ cha của tập hợp thẻ chứa usercontrol. Vì vậy, với 
        //command đóng, ta phải duyệt lần lượt từng thẻ cha của usercontrol cho đến khi tìm được window, từ đó truyền window
        //vào command đóng.
        FrameworkElement GetWindowParent (UserControl p)
        {
            FrameworkElement parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement; 
            }

            return parent;
        }
    }
}
