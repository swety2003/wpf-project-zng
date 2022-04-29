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
using WPF_Project.Pages;

namespace WPF_Project
{
    /// <summary>
    /// CustomDialog.xaml 的交互逻辑
    /// </summary>
    public partial class CustomDialog : Window
    {
        public string MTitle { get; set; } = "系统提示";
        public string Msg { get; set; } = "提示信息";
        public string YBtn { get; set; } = "确认";
        public string NBtn { get; set; } = "取消";
        public CustomDialog(string title,string msg,string[] btnText=null)
        {
            InitializeComponent();
            this.MTitle = title;
            this.Msg = msg;
            if (btnText != null)
            {

                if (btnText.Length == 2)
                {

                    this.YBtn = btnText[0];
                    this.NBtn = btnText[1];
                }
            }

            this.DataContext = this;
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
}
