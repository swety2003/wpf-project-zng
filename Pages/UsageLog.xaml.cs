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
    /// UsageLog.xaml 的交互逻辑
    /// </summary>
    public partial class UsageLog : Page
    {
        public UsageLog()
        {
            InitializeComponent();
            this.DataContext = new UsageLogVM();
        }
    }

    public class UsageLogVM : ObservableObject
    {
        public string cabinetgroupId { get; set; }

        public UsageLogVM()
        {
            PageLoadedCMD = new RelayCommand(PageLoaded);
        }

        public ICommand PageLoadedCMD { get; }
        public ICommand ShowDetailCMD { get; }

        private API.ToolGet.UserLogDT.Root _allTools;

        public API.ToolGet.UserLogDT.Root AllTools
        {
            get => _allTools;
            private set => SetProperty(ref _allTools, value);
        }

        private void PageLoaded()
        {
            this.cabinetgroupId = StaticValues.TryGetCabID();
            StaticValues.MainWindow.topBar.Visibility = Visibility.Visible;
            StaticValues.MainWindow.SetTitle("使用记录");

            LoadDataAsync();

        }
        private async Task LoadDataAsync()
        {
            var r = await API.ToolGet.UserLogGet("1", "10");

            AllTools = JsonConvert.DeserializeObject<API.ToolGet.UserLogDT.Root>(r);




        }




    }

}
