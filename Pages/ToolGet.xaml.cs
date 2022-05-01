using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using WPF_Project.Utils;

namespace WPF_Project.Pages
{
    /// <summary>
    /// ToolGet.xaml 的交互逻辑
    /// </summary>
    public partial class ToolGet : Page
    {

        public TooGetVM vm = new();
        public ToolGet()
        {
            InitializeComponent();

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {



            var Selected = vm.ToolsByTool.rows.Where(item => item.selected == true).ToList();



            OPenDoorHandlerAsync();





            //new ToolGetDialog1("系统提示", "门已打开，请取出工具后关闭柜门").ShowDialog();
        }

        private async Task OPenDoorHandlerAsync()
        {

            try
            {
                RWEPC rwepc = new RWEPC("192.168.0.7", "20001");


                rwepc.Connect();

                rwepc.Connected= true;

                Thread.Sleep(2000);



                rwepc.EndScan();
                var before = await rwepc.GetEpcInfo();


                rwepc.DisConnect();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
















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

            this.DataContext = vm;

            LoadDataAsync();

        }

        private async Task LoadDataAsync()
        {
            var r = await API.ToolGet.ByTool("1", "10", "7");

            vm.ToolsByTool = JsonConvert.DeserializeObject<API.ToolGet.DataType.Root>(r);


        }
    }

    public class TooGetVM : NotifyBase
    {
        private API.ToolGet.DataType.Root _toolsByTool;

        public API.ToolGet.DataType.Root ToolsByTool
        {
            get { return _toolsByTool; }
            set { _toolsByTool = value; DoNotify(); }
        }
    }
}
