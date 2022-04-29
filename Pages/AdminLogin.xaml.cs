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
    /// AdminLogin.xaml 的交互逻辑
    /// </summary>
    public partial class AdminLogin : Page
    {
        public AdminLogin()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(passwdBox.Password== "chongdao123")
            {
                StaticValues.MainWindow.NagivateTo(new AdminView());

            }
            else
            {
                new ToolGetDialog1("系统提示","密码不正确！").ShowDialog();
                passwdBox.Password = "";
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StaticValues.MainWindow.SetTitle("系统维护");
            StaticValues.Toparea.Visibility = Visibility.Visible;

        }
    }
}
