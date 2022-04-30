using Newtonsoft.Json;
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

namespace WPF_Project.Pages
{
    /// <summary>
    /// ParamConfig.xaml 的交互逻辑
    /// </summary>
    public partial class ParamConfig : Page
    {

        public ParamConfigVM vm=new ParamConfigVM();
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
            var r =await Switch.switchList("1", "1", "1");

            vm.SwitchList = JsonConvert.DeserializeObject<Switch.switchListDataType.Root>(r);


        }


        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("233");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            vm.SwitchList.rows.Remove(switchDG.SelectedValue as Switch.switchListDataType.RowsItem);
            RefreshDataGrid();

        }

        public void RefreshDataGrid()
        {
            switchDG.ItemsSource = null;
            switchDG.ItemsSource = vm.SwitchList.rows;
        }

        //添加
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            vm.SwitchList.rows.Add(new Switch.switchListDataType.RowsItem
            {
                switchName="测试",
                switchIp="",
                switchPort="",
                cabinetgroupId=1,
            });
            RefreshDataGrid();
        }
    }

    public class ParamConfigVM :NotifyBase
    {
        private Switch.switchListDataType.Root _switchList;

        public Switch.switchListDataType.Root SwitchList
        {
            get { return _switchList; }
            set { _switchList = value;DoNotify(); }
        }

    }
}
