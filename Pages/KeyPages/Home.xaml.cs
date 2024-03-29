﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
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
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            DataContext=new HomeVM();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StaticValues.MainWindow.NavigateTo(new ParamConfig());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StaticValues.MainWindow.NavigateTo(new Tasks());

        }
    }

    public class HomeVM : ObservableObject
    {
        public string cabinetgroupId { get; set; }

        public HomeVM()
        {
            PageLoadedCMD = new RelayCommand(PageLoaded);
            PageUnloadedCMD = new RelayCommand(PageUnloaded);
        }

        public ICommand PageLoadedCMD { get; }
        public ICommand ShowDetailCMD { get; }
        public ICommand PageUnloadedCMD { get; }


        private void PageLoaded()
        {
            StaticValues.Toparea.Visibility = Visibility.Collapsed;
            StaticValues.MainWindow.HideHomeBtn();

        }

        private void PageUnloaded()
        {
            StaticValues.MainWindow.ShowHomeBtn();

        }



    }

}
