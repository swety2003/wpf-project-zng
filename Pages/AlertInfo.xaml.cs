using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
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
using WPF_Project.Common;

namespace WPF_Project.Pages
{
    /// <summary>
    /// AlertInfo.xaml 的交互逻辑
    /// </summary>
    public partial class AlertInfo : Page
    {
        public AlertInfo()
        {
            InitializeComponent();
            DataContext = new AlertInfoVM();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class AlertInfoVM : ObservableObject
    {
        public string cabinetgroupId { get; set; }

        public AlertInfoVM()
        {
            PageLoadedCMD = new RelayCommand(PageLoaded);
            ShowDetailCMD = new RelayCommand<string>((id) =>
            {

            });
        }

        public ICommand PageLoadedCMD { get; }
        public ICommand ShowDetailCMD { get; }

        private API.ToolGet.AlertInfoDT.Root _allTools;

        public API.ToolGet.AlertInfoDT.Root AllAlerts
        {
            get => _allTools;
            private set => SetProperty(ref _allTools, value);
        }

        private void PageLoaded()
        {

            StaticValues.MainWindow.topBar.Visibility = Visibility.Visible;
            StaticValues.MainWindow.SetTitle("报警查询");

            LoadDataAsync();

        }
        private async Task LoadDataAsync()
        {
            var r = await API.ToolGet.GetAlertInfo("1", "10");

            AllAlerts = JsonConvert.DeserializeObject<API.ToolGet.AlertInfoDT.Root>(r);




        }

        private void ShowDetail(int id)
        {
        }



    }

}
