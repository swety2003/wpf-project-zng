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
    public partial class SimpleInput : Window
    {
        public InputVm vm=new InputVm();
        public SimpleInput()
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        public string[] Result
        {
            get { return new string[] { vm.IP,vm.PORT }; }
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

    public class InputVm:NotifyBase
    {
        private string _ip;

        public string IP
        {
            get { return _ip; }
            set { _ip = value; DoNotify(); }
        }

        private string _port;

        public string PORT
        {
            get { return _port; }
            set { _port = value;DoNotify(); }
        }

    }
}
