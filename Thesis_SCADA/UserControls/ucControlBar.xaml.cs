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
using Thesis_SCADA.ViewModel;

namespace Thesis_SCADA.UserControls
{
    /// <summary>
    /// Interaction logic for ucControlBar.xaml
    /// </summary>
    public partial class ucControlBar : UserControl
    {
        //Để mỗi cửa sổ sẽ được gắn một control bar riêng, control bar phải được tạo mới trong uc, không 
        //thể tạo mới bằng móc nối với từng view được
        //Ý nghĩa: mỗi view sẽ được gắn với một viewmodel riêng, không dùng chung viewmodel. rất có ý nghĩa 
        //khi dùng usercontrol
        public ControlBarViewModel ViewModel { get; set; }
        public ucControlBar()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ControlBarViewModel();
        }
    }
}
