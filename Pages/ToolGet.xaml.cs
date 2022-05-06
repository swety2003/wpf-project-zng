using IntellectTestTool.UHFReader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfWidgetDesktop.Utils;

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

        public string cabinetgroupId;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {



            var Selected = vm.ToolsByTool.rows.Where(item => item.selected == true).ToList();



            OPenDoorHandlerAsync(Selected);





            //new ToolGetDialog1("系统提示", "门已打开，请取出工具后关闭柜门").ShowDialog();
        }

        //比对
        private List<EpcInfo> Compare(List<EpcInfo> max, List<EpcInfo>min)
        {
            var result = new List<EpcInfo>();

            foreach (var item in max)
            {
                if (!min.Contains(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        //打开天线读数据
        private async Task<List<EpcInfo>> ScanForEpcInfoAsync(String ip, string port)
        {
            List<EpcInfo> result = new List<EpcInfo>();
            RWEPC rwepc = new RWEPC(ip,port);


            rwepc.Connect();

            rwepc.Connected = true;

            Thread.Sleep(2000);



            rwepc.EndScan();
            result = await rwepc.GetEpcInfo();


            rwepc.DisConnect();
            return result;
        }


        private async Task OPenDoorHandlerAsync(List<API.ToolGet.ByToolDataType.RowsItem> items)
        {
            foreach (var item in items)
            {
                var detail = API.ToolGet.GetGridDetailById("");

            }





            try
            {
                //打开前
                var before=await ScanForEpcInfoAsync("192.168.0.7", "20001");

                //开关
                Switch switchModbus = new Switch("192.168.0.12", "502");
                switchModbus.connectAndOpenDoor("01");
                switchModbus.DisConnect();

                //打开后
                var after =await ScanForEpcInfoAsync("192.168.0.7", "20001");

                


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

            var r=SettingProvider.Get("core.paramcfg");

            JObject o=JObject.Parse(r);

            cabinetgroupId =(string) o["cabinetgroupId"];

            if (cabinetgroupId!=null)
            {
                LoadDataAsync();

            }
            else
            {
                MessageBox.Show("未设置本机柜组id");
            }






        }

        private async Task LoadDataAsync()
        {
            var r = await API.ToolGet.ByTool("1", "10", cabinetgroupId);

            vm.ToolsByTool = JsonConvert.DeserializeObject<API.ToolGet.ByToolDataType.Root>(r);


        }


        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {

            if (cabinetgroupId != null)
            {
                LoadCabAsync();
            }




        }

        private async void LoadCabAsync()
        {
            var r = await API.ToolGet.Toolsubcabinet("1", "10", cabinetgroupId);

            vm.SubCabList = JsonConvert.DeserializeObject<API.ToolGet.subcabinetDT.Root>(r);

            vm.TotalCab=vm.SubCabList.total;

            if (vm.TotalCab>=1)
            {
                if (vm.CurrentCab == 0)
                {
                    vm.CurrentCab = 1;

                }

                var SelectedSubCab = vm.SubCabList.rows[vm.CurrentCab-1];

                vm.CabName = SelectedSubCab.subcabinetName;

                LoadCabGridAsync(SelectedSubCab.subcabinetId.ToString());
            }
            else
            {
                vm.CabName = "没有柜组";
            }
        }

        private async Task LoadCabGridAsync(string subcabinetId)
        {
            var r = await API.ToolGet.ToolcabinetGrid("1", "10", subcabinetId);

            vm.CabGridList = JsonConvert.DeserializeObject<API.ToolGet.cabinetgridDT.Root>(r);
        }



        private void CabListItem_BTNClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("clicked");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (vm.CurrentCab>1)
            {
                vm.CurrentCab -= 1;
                LoadCabAsync();
            }
            else
            {

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (vm.CurrentCab < vm.TotalCab)
            {
                vm.CurrentCab += 1;
                LoadCabAsync();

            }
            else
            {

            }

        }
    }

    public class TooGetVM : NotifyBase
    {
        private int _currentcab;

        public int CurrentCab
        {
            get { return _currentcab; }
            set { _currentcab = value;DoNotify(); }
        }
        private int _totalCab;

        public int TotalCab
        {
            get { return _totalCab; }
            set { _totalCab = value; DoNotify(); }
        }

        private string _cabName;

        public string CabName
        {
            get { return _cabName; }
            set { _cabName = value; DoNotify(); }
        }







        private API.ToolGet.ByToolDataType.Root _toolsByTool;

        public API.ToolGet.ByToolDataType.Root ToolsByTool
        {
            get { return _toolsByTool; }
            set { _toolsByTool = value; DoNotify(); }
        }


        private API.ToolGet.subcabinetDT.Root _subCabList;

        public API.ToolGet.subcabinetDT.Root SubCabList
        {
            get { return _subCabList; }
            set { _subCabList = value; DoNotify(); }
        }



        private API.ToolGet.cabinetgridDT.Root _cabGrid;

        public API.ToolGet.cabinetgridDT.Root CabGridList
        {
            get { return _cabGrid; }
            set { _cabGrid = value; DoNotify(); }
        }


    }
}
