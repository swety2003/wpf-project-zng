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
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StaticValues.MainWindow.NavigateTo(new ToolGet());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StaticValues.MainWindow.NavigateTo(new AdminLogin(),false);

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StaticValues.Toparea.Visibility = Visibility.Collapsed;
            StaticValues.MainWindow.HideHomeBtn();


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {


            StaticValues.MainWindow.NavigateTo(new ToolInfo());

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            StaticValues.MainWindow.NavigateTo(new UsageLog());

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            StaticValues.MainWindow.ShowHomeBtn();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            StaticValues.MainWindow.NavigateTo(new AlertInfo());

        }
    }
}
