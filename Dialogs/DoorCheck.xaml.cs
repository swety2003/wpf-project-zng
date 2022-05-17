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
using System.Windows.Shapes;
using WPF_Project.Common;
using WPF_Project.Pages;
using WPF_Project.Utils;

namespace WPF_Project
{
    /// <summary>
    /// CustomDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DoorCheck : Window
    {
        Switch sw;
        string inputPoint;
        public static Action Print_Action;//声明事件
        public DoorCheck(Switch sw,string inputPoint)
        {
            InitializeComponent();
            this.sw = sw;
            this.DataContext = this;
            this.inputPoint = inputPoint;

            PrintAsync();
        }






        public async Task PrintAsync()
        {
            while (true)
            {
                await Task.Delay(1000);
                var r = sw.checkDoorStatus(inputPoint);
                Console.WriteLine(r);
                if (r == false)
                {
                    break;
                }
            }


            this.Close();


        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //DialogResult = false;
            //this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //DialogResult = true;
            //this.Close();
        }
    }
}
