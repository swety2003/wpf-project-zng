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
    /// HomePageView.xaml 的交互逻辑
    /// </summary>
    public partial class HomePageView : Page
    {
        public HomePageView()
        {
            InitializeComponent();
            StaticValues.Toparea.Visibility = Visibility.Collapsed;
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new CustomDialog().ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StaticValues.MainWindow.NagivateTo(new ToolGet());
            StaticValues.MainWindow.topBar.Visibility = Visibility.Visible;
            StaticValues.MainWindow.SetTitle("工具取还");
        }
    }
}
