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
using System.Windows.Shapes;
using WPF_Project.Common;

namespace WPF_Project.Dialogs
{
    /// <summary>
    /// SimpleInput.xaml 的交互逻辑
    /// </summary>
    public partial class UserLogin : Window
    {
        public UserLoginVm vm = new();
        public UserLogin()
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        public string[] Result
        {
            get { return new string[] { vm.ID,vm.PWD }; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

    }

    public class UserLoginVm : NotifyBase
    {
        private string _ip;

        public string ID
        {
            get { return _ip; }
            set { _ip = value; DoNotify(); }
        }

        private string _port;

        public string PWD
        {
            get { return _port; }
            set { _port = value;DoNotify(); }
        }

    }
}
