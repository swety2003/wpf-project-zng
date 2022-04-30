using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using WPF_Project.API;
using WPF_Project.Common;
using WPF_Project.Dialogs;
using WpfWidgetDesktop.Utils;

namespace WPF_Project.Pages
{
    /// <summary>
    /// ParamConfig.xaml 的交互逻辑
    /// </summary>
    public partial class ParamConfig : Page
    {

        public ParamConfigVM vm = new ParamConfigVM();
        public ParamConfig()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            StaticValues.MainWindow.SetTitle("参数配置");
            StaticValues.Toparea.Visibility = Visibility.Visible;
            this.DataContext = vm;

            LoadDataAsync();
        }


        private async Task LoadDataAsync()
        {
            try
            {
                var cfg = JObject.Parse(SettingProvider.Get("core.admin"));

                if (cfg != null)
                {
                    vm.SysIP = (string)cfg["ip"];
                    vm.SysPort = (string)cfg["port"];
                }
            }
            catch (Exception ex)
            {
                
            }

            var r = await Switch.switchList("1", "1", "1");

            vm.SwitchList = JsonConvert.DeserializeObject<Switch.switchListDataType.Root>(r);

            r = await CabinetGroup.GroupList("1", "10");

            vm.CabinetGroupList = JsonConvert.DeserializeObject<CabinetGroup.GroupDataType.Root>(r);

            r = await antennagroup.GroupList("1", "1", "1");

            vm.AntennaGroup = JsonConvert.DeserializeObject<antennagroup.GroupDataType.Root>(r);



        }


        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async Task AddSwitchAsync(string[] r)
        {
            var resp = await Switch.switchListAdd(r[0], r[1], r[2], r[3]);

            try
            {

                JObject o = JObject.Parse(resp);

                int code = (int)o["code"];

                if (code == 200)
                {

                    new ToolGetDialog1("系统提示", "添加成功！").ShowDialog();

                    LoadDataAsync();

                }
                else
                {
                    new ToolGetDialog1("系统提示", "添加失败！").ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("网络错误/服务端错误!");
            }

        }



        //开关
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {


            var d = new SwitchEdit();
            var r = d.ShowDialog();
            if (r == true)
            {
                AddSwitchAsync(d.Result);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            vm.SwitchList.rows.Remove(switchDG.SelectedValue as Switch.switchListDataType.RowsItem);

        }

        //天线组操作
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }
    }

    public class ParamConfigVM : NotifyBase
    {
        private Switch.switchListDataType.Root _switchList;

        public Switch.switchListDataType.Root SwitchList
        {
            get { return _switchList; }
            set { _switchList = value; DoNotify(); }
        }

        private CabinetGroup.GroupDataType.Root _cabinetGroupList;

        public CabinetGroup.GroupDataType.Root CabinetGroupList
        {
            get { return _cabinetGroupList; }
            set { _cabinetGroupList = value; DoNotify(); }
        }

        private antennagroup.GroupDataType.Root _antennaGroup;

        public antennagroup.GroupDataType.Root AntennaGroup
        {
            get { return _antennaGroup; }
            set { _antennaGroup = value; }
        }


        private string _sysip;

        public string SysIP
        {
            get { return _sysip; }
            set { _sysip = value;DoNotify(); }
        }

        private string _sysPORT;

        public string SysPort
        {
            get { return _sysPORT; }
            set { _sysPORT = value; DoNotify(); }
        }



    }
}
