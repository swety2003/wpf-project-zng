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

namespace WPF_Project.Dialogs
{
    /// <summary>
    /// CustomDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ToolInfo : Window
    {
        public string MTitle { get; set; } = "详细信息";
        public string YBtn { get; set; } = "确认";
        public string NBtn { get; set; } = "关闭";
        public API.ToolGet.ByToolDataType.RowsItem item { get; set; }
        public ToolInfo(API.ToolGet.ByToolDataType.RowsItem item)
        {
            InitializeComponent();
            this.item = item;

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
