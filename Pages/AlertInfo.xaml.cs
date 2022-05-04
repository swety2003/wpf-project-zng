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
    }

    public class AlertInfoVM : ObservableObject
    {
        public string cabinetgroupId { get; set; }

        public AlertInfoVM()
        {
            PageLoadedCMD = new RelayCommand(PageLoaded);
            ShowDetailCMD = new RelayCommand<string>((id) =>
            {

                API.ToolGet.ByToolDataType.RowsItem selectedItem = null;
                foreach (var item in AllTools.rows)
                {
                    if (item.toolId.ToString() == id.ToString())
                    {
                        selectedItem = item;
                    }
                }
                if (selectedItem != null)
                {
                    new Dialogs.ToolInfo(item: selectedItem).ShowDialog();
                }
            });
        }

        public ICommand PageLoadedCMD { get; }
        public ICommand ShowDetailCMD { get; }

        private API.ToolGet.ByToolDataType.Root _allTools;

        public API.ToolGet.ByToolDataType.Root AllTools
        {
            get => _allTools;
            private set => SetProperty(ref _allTools, value);
        }

        private void PageLoaded()
        {
            this.cabinetgroupId = StaticValues.TryGetCabID();
            StaticValues.MainWindow.topBar.Visibility = Visibility.Visible;
            StaticValues.MainWindow.SetTitle("工具查询");

            if (this.cabinetgroupId != null)
            {
                LoadDataAsync();
            }

        }
        private async Task LoadDataAsync()
        {
            var r = await API.ToolGet.ByTool("1", "10", cabinetgroupId);

            AllTools = JsonConvert.DeserializeObject<API.ToolGet.ByToolDataType.Root>(r);




        }

        private void ShowDetail(int id)
        {
        }



    }

}
