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
    public partial class SwitchEdit : Window
    {
        public SwitchEditVm vm = new();
        public SwitchEdit(string[] r=null)
        {
            InitializeComponent();
            this.DataContext = vm;

            if (r != null)
            {
                vm.Name= r[0];
                vm.IP= r[1];
                vm.PORT= r[2];
                vm.ID= r[3];
            }
        }

        public string[] Result
        {
            get { return new string[] { vm.Name,vm.IP,vm.PORT,vm.ID }; }
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

    public class SwitchEditVm : NotifyBase
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; DoNotify(); }
        }

        private string _id;

        public string ID 
        {
            get { return _id; }
            set { _id = value; }
        }


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
