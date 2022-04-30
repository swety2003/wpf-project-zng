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
using System.Windows.Threading;
using WPF_Project.Common;
using WPF_Project.Pages;
using WpfWidgetDesktop.Utils;

namespace WPF_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer dt;
        private MainVM vm=new();
        public bool IsLogin = false;
        public MainWindow()
        {

            InitializeComponent();
            StaticValues.MainWindow = this;
            StaticValues.Toparea = topBar;
            StaticValues.scanBtn = scanBtn;
            this.DataContext = vm;
            dt = new DispatcherTimer();

            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dt_Tick;
            dt.Start();
        }
        public void ChangeBackGround(string url)
        {
            windowBg.ImageSource = new BitmapImage(new Uri(url, UriKind.Relative));
        }
        public void LoginSuccess()
        {
            LoginSuccessBtn.Visibility = Visibility.Visible;
            loginLabel.Visibility = Visibility.Collapsed;
        }
        public void SetTitle(string title)
        {
            titleTB.Text = title;

        }
        public void NagivateTo(Page p,bool ensuerLogin=true)
        {
            if (ensuerLogin)
            {

                if (!IsLogin)
                {
                    EnsuerLogin();
                    return;
                }
            }
            rootFrame.Navigate(p);

        }
        public string Week()
        {
            string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string week = weekdays[Convert.ToInt32(DateTime.Now.DayOfWeek)];

            return week;
        }
        void dt_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            var y = now.ToString("yyyy.M.dd");
            var t = now.ToString("t");
            vm.CTime = $"{y}  {Week()}  {t}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (rootFrame.NavigationService.CanGoBack)
            {
                rootFrame.NavigationService.GoBack();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var r = new CustomDialog("系统提示","是否退出系统？").ShowDialog();
            if(r == true)
            {

                SettingProvider.Save();
                Environment.Exit(0);

            }
        }

        private void LoginSuccessBtn_Click(object sender, RoutedEventArgs e)
        {
            StaticValues.MainWindow.NagivateTo(new HomePageView());
        }

        private void loginLabel_Click(object sender, RoutedEventArgs e)
        {
            EnsuerLogin();
        }
        public bool AcquireLogin()
        {
            return true;
        }
        public bool EnsuerLogin()
        {

            if (!IsLogin)
            {

                new ToolGetDialog1("系统提示","请刷卡或扫脸登录").ShowDialog();

                IsLogin = AcquireLogin();
                if (IsLogin)
                {
                    LoginSuccess();

                }
                var r=new CustomDialog("系统提示" ,"欢迎你，张三\n 是否快速取还？").ShowDialog();
                if (r==true)
                {
                    StaticValues.MainWindow.NagivateTo(new ToolGet());

                }

            }
            else
            {
                LoginSuccess();
            }
            return IsLogin;


        }
    }
    public class MainVM:NotifyBase
    {
        private string _ctime;

        public string CTime
        {
            get { return _ctime; }
            set { _ctime = value; DoNotify(); }
        }


    }
}
