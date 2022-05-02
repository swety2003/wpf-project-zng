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
        private string guid = "core.paramcfg";

        public class CFG
        {
            public bool IsKey { get; set; }

            public string cabinetgroupId { get; set; }
        }




        //选择的柜组id
        private string cabinetgroupId;

        private CFG cfg;

        public ParamConfigVM vm = new ParamConfigVM();
        public ParamConfig()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                var r=SettingProvider.Get(guid);
                cfg = JsonConvert.DeserializeObject<CFG>(r);
                if (cfg!= null)
                {
                    cabinetgroupId = cfg.cabinetgroupId;

                    modeSelectRB.IsChecked=!cfg.IsKey;
                }
                else
                {
                    cfg = new CFG { IsKey=false};
                }
            }catch (Exception ex) {
                    cfg = new CFG { IsKey=false};
            }


            StaticValues.MainWindow.SetTitle("参数配置");
            StaticValues.Toparea.Visibility = Visibility.Visible;

            



            this.DataContext = vm;

            LoadDataAsync();
        }


        private async Task LoadDataAsync(bool refreshAcbinetGroup = true)
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
            if (refreshAcbinetGroup)
            {

                var r = await CabinetGroup.GroupList("1", "10");

                vm.CabinetGroupList = JsonConvert.DeserializeObject<CabinetGroup.GroupDataType.Root>(r);
                foreach(var item in vm.CabinetGroupList.rows)
                {
                    if (item.cabinetgroupId.ToString() == cabinetgroupId)
                    {
                        cabinetgroupIdCB.SelectedItem = item;
                        break;
                    }
                }
            }

            if (cabinetgroupId != null)
            {

                var r = await Switch.switchList("1", "10", cabinetgroupId);

                vm.SwitchList = JsonConvert.DeserializeObject<Switch.switchListDataType.Root>(r);


                r = await antennagroup.GroupList("1", "10", cabinetgroupId);

                vm.AntennaGroup = JsonConvert.DeserializeObject<antennagroup.GroupDataType.Root>(r);

                r=await ToolDoor.Get("1","10",cabinetgroupId);
                var origin= JsonConvert.DeserializeObject<ToolDoor.DataType.Root>(r);


                vm.ToolDoor = new ToolDoor.DataType.RowsItem();
                foreach (var item in origin.rows)
                {
                    if(item.cabinetgroupId.ToString() == cabinetgroupId)
                    {
                        IsDoorNull = false;
                        vm.ToolDoor = item;
                    }
                }
                if (IsDoorNull)
                {
                    vm.ToolDoor = new();
                }
                


            }


        }


        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
        //switch编辑
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString();
            API.Switch.switchListDataType.RowsItem selected = null;
            foreach (var a in vm.SwitchList.rows)
            {
                if (a.switchId.ToString() == id)
                {
                    selected = a;
                    break;
                }


            }

            if (selected != null)
            {
                EditSwitchAsync(selected);
            }
        }

        private async Task EditSwitchAsync(API.Switch.switchListDataType.RowsItem selected)
        {

            var d = new SwitchEdit(title: "开关量编辑", r: new string[]{
                selected.switchName,
                selected.switchIp,
                selected.switchPort,
                selected.cabinetgroupId.ToString()

            });
            var rd = d.ShowDialog();
            if (rd == true)
            {
                var r = d.Result;
                var antennagroupId = selected.switchId.ToString();
                var resp = await API.Switch.Edit(r[0], r[1], r[2], r[3], antennagroupId);

                try
                {

                    JObject o = JObject.Parse(resp);

                    int code = (int)o["code"];

                    if (code == 200)
                    {

                        new ToolGetDialog1("系统提示", "编辑成功！").ShowDialog();

                        LoadDataAsync(false);

                    }
                    else
                    {
                        new ToolGetDialog1("系统提示", "编辑失败！").ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("网络错误/服务端错误!");
                }
            }
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

                    LoadDataAsync(false);

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



        //开关 add
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {


            var d = new SwitchEdit();
            var r = d.ShowDialog();
            if (r == true)
            {
                AddSwitchAsync(d.Result);
            }
        }


        //开关删除
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString();
            DoDelectSwitchAsync(id);

        }

        private async Task DoDelectSwitchAsync(string id)
        {

            await API.Switch.switchDel(id);

            LoadDataAsync(false);
            //vm.SwitchList.rows.Remove(switchDG.SelectedValue as Switch.switchListDataType.RowsItem);
        }

        //天线组编辑
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString();
            API.antennagroup.GroupDataType.RowsItem selected = null;
            foreach (var a in vm.AntennaGroup.rows)
            {
                if (a.antennagroupId.ToString() == id)
                {
                    selected = a;
                    break;
                }


            }

            if (selected != null)
            {
                EditAntennaAsync(selected);
            }
        }

        private async Task EditAntennaAsync(API.antennagroup.GroupDataType.RowsItem selected)
        {

            var d = new SwitchEdit(title: "天线组编辑", r: new string[]{
                selected.antennagroupName,
                selected.antennagroupIp,
                selected.antennagroupPort,
                selected.cabinetgroupId.ToString()

            });
            var rd = d.ShowDialog();
            if (rd == true)
            {
                var r = d.Result;
                var antennagroupId = selected.antennagroupId.ToString();
                var resp = await antennagroup.Edit(r[0], r[1], r[2], r[3], antennagroupId);

                try
                {

                    JObject o = JObject.Parse(resp);

                    int code = (int)o["code"];

                    if (code == 200)
                    {

                        new ToolGetDialog1("系统提示", "编辑成功！").ShowDialog();

                        LoadDataAsync(false);

                    }
                    else
                    {
                        new ToolGetDialog1("系统提示", "编辑失败！").ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("网络错误/服务端错误!");
                }
            }
        }



        //天线组删除
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString();
            DoDeletantennaAsync(id);
        }

        private async Task DoDeletantennaAsync(string id)
        {

            await API.antennagroup.Del(id);

            LoadDataAsync(false);
            //vm.SwitchList.rows.Remove(switchDG.SelectedValue as Switch.switchListDataType.RowsItem);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var cb=sender as ComboBox;

            var selected = e.AddedItems[0] as API.CabinetGroup.GroupDataType.RowsItem;
            cabinetgroupId = selected.cabinetgroupId.ToString(); ;

            LoadDataAsync(false);



        }


        //天线组 add
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var d = new SwitchEdit(title: "天线组编辑");
            var r = d.ShowDialog();
            if (r == true)
            {
                AddantennaAsync(d.Result);
            }
        }

        private async Task AddantennaAsync(string[] r)
        {
            var resp = await antennagroup.Add(r[0], r[1], r[2], r[3]);

            try
            {

                JObject o = JObject.Parse(resp);

                int code = (int)o["code"];

                if (code == 200)
                {

                    new ToolGetDialog1("系统提示", "添加成功！").ShowDialog();

                    LoadDataAsync(false);

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

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            cfg.cabinetgroupId = cabinetgroupId;
            cfg.IsKey = modeSelectRB.IsChecked==false;
            SettingProvider.Set(guid, cfg);
        }



        private bool IsDoorNull = true;
        //保存门禁信息
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

            SaveDoorAsync();

        }
        private async void SaveDoorAsync()
        {
            string resp="";
            if (IsDoorNull)
            {

                resp=await ToolDoor.Add(doorName:vm.ToolDoor.doorName,
                    doorIp:vm.ToolDoor.doorIp,doorPort:vm.ToolDoor.doorPort,
                    account:vm.ToolDoor.account,password:vm.ToolDoor.password,cabinetgroupId:cabinetgroupId);


            }
            else
            {
                resp = await ToolDoor.Edit(doorName: vm.ToolDoor.doorName,
                    doorIp: vm.ToolDoor.doorIp, doorPort: vm.ToolDoor.doorPort,
                    account: vm.ToolDoor.account, password: vm.ToolDoor.password, cabinetgroupId: cabinetgroupId,
                    doorId: vm.ToolDoor.doorId.ToString());



            }
            try
            {

                JObject o = JObject.Parse(resp);

                int code = (int)o["code"];

                if (code == 200)
                {

                    new ToolGetDialog1("系统提示", "编辑成功！").ShowDialog();

                    LoadDataAsync(false);

                }
                else
                {
                    new ToolGetDialog1("系统提示", "编辑失败！").ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("网络错误/服务端错误!");
            }
        }
    }

    public class ParamConfigVM : NotifyBase
    {

        private ToolDoor.DataType.RowsItem _toolDoor;

        public ToolDoor.DataType.RowsItem ToolDoor
        {
            get { return _toolDoor; }
            set { _toolDoor = value;DoNotify(); }
        }



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
            set { _antennaGroup = value; DoNotify(); }
        }


        private string _sysip;

        public string SysIP
        {
            get { return _sysip; }
            set { _sysip = value; DoNotify(); }
        }

        private string _sysPORT;

        public string SysPort
        {
            get { return _sysPORT; }
            set { _sysPORT = value; DoNotify(); }
        }



    }
}
