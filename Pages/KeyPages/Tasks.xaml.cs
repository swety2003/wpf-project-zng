using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
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

namespace WPF_Project.Pages.KeyPages
{
    /// <summary>
    /// Home.xaml 的交互逻辑
    /// </summary>
    public partial class Tasks : Page
    {
        public Tasks()
        {
            InitializeComponent();
            DataContext=new TasksVM();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
    }

    public class TasksVM : ObservableObject
    {
        public string cabinetgroupId { get; set; }

        public TasksVM()
        {
            PageLoadedCMD = new RelayCommand(PageLoaded);
            PageUnloadedCMD = new RelayCommand(PageUnloaded);
        }

        public ICommand PageLoadedCMD { get; }
        public ICommand ShowDetailCMD { get; }
        public ICommand PageUnloadedCMD { get; }


        private void PageLoaded()
        {
            StaticValues.Toparea.Visibility = Visibility.Visible;
            StaticValues.MainWindow.SetTitle("工作台");
            //StaticValues.MainWindow.ShowHomeBtn();

        }

        private void PageUnloaded()
        {
            //StaticValues.MainWindow.ShowHomeBtn();

        }



    }

}
