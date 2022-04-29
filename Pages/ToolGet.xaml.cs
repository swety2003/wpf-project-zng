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
    /// ToolGet.xaml 的交互逻辑
    /// </summary>
    public partial class ToolGet : Page
    {
        public ToolGet()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ToolGetDialog1("系统提示", "门已打开，请取出工具后关闭柜门").ShowDialog();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            StaticValues.scanBtn.Visibility = Visibility.Hidden;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            StaticValues.MainWindow.topBar.Visibility = Visibility.Visible;
            StaticValues.MainWindow.SetTitle("工具取还");
            StaticValues.scanBtn.Visibility = Visibility.Visible;
        }
    }
}
