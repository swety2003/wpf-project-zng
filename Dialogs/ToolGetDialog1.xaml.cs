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

namespace WPF_Project
{
    /// <summary>
    /// CustomDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ToolGetDialog1 : Window
    {
        public string MTitle { get; set; } = "系统提示";
        public string Msg { get; set; } = "提示信息";
        public ToolGetDialog1(string title, string msg)
        {
            InitializeComponent();
            this.MTitle = title;
            this.Msg = msg;
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
