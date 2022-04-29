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
using WPF_Project.Common;

namespace WPF_Project.Pages
{
    /// <summary>
    /// AdminManage.xaml 的交互逻辑
    /// </summary>
    public partial class AdminManage : Page
    {
        public AdminManage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StaticValues.MainWindow.SetTitle("管理员");
            StaticValues.Toparea.Visibility = Visibility.Visible;
        }
    }
}
