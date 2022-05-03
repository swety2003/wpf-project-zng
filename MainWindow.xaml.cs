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
using System.Windows.Threading;
using WPF_Project.Common;
using WPF_Project.Dialogs;
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
                    EnsuerLoginAsync();
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
        public void ClearHistory()
        {
            if (!rootFrame.CanGoBack && !rootFrame.CanGoForward)
            {
                return;
            }

            var entry = rootFrame.RemoveBackEntry();
            while (entry != null)
            {
                entry = rootFrame.RemoveBackEntry();
            }

            rootFrame.Navigate(new PageFunction<string>() { RemoveFromJournal = true });
        }
        private void LoginSuccessBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearHistory();

            StaticValues.MainWindow.NagivateTo(new HomePageView());
        }

        private void loginLabel_Click(object sender, RoutedEventArgs e)
        {
            EnsuerLoginAsync();
        }
        public bool AcquireLogin()
        {
            return true;
        }
        public async Task EnsuerLoginAsync()
        {
#if DEBUG
            if (!IsLogin)
            {
                API.APICOMMON.SetToken("eyJhbGciOiJIUzUxMiJ9.eyJsb2dpbl91c2VyX2tleSI6ImJlMzcwOGMyLTA1MzEtNDkwZS05OWNmLTBmYmYyNGE1MDI1ZCJ9.aI73IMVZUSPZ-N39ahHO6YAcUvtD5VQRB27Do0YOoED2Aoz6Gh2ZyA5vaq-UFSeLiLBkjhxQoo0UbN7eO_P6dA");
                IsLogin = true;
            }
#else

            if (!IsLogin)
            {

                //new ToolGetDialog1("系统提示","请刷卡或扫脸登录").ShowDialog();


                var d = new UserLogin();
                var dr = d.ShowDialog();
                if (dr == true)
                {
                    var resp=await API.UserLogin.LoginAsync(d.Result[0],d.Result[1]);

                    JObject jo = JObject.Parse(resp);

                    try
                    {

                        int code = (int)jo["code"];

                        string token=(string)jo["token"];



                        if (code == 200)
                        {
                            StaticValues.MainWindow.IsLogin = true;

                            StaticValues.MainWindow.LoginSuccess();

                            API.APICOMMON.SetToken(token);


                            var r = new CustomDialog("系统提示", "欢迎你，张三\n 是否快速取还？").ShowDialog();
                            if (r == true)
                            {
                                StaticValues.MainWindow.NagivateTo(new ToolGet());

                            }

                        }
                        else
                        {
                            new ToolGetDialog1("系统提示", "密码不正确！").ShowDialog();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("网络错误/服务端错误!");
                    }


                }


            }
            else
            {
                //LoginSuccess();
            }

#endif

        }

        private void LoginSuccessBtn_Click_1(object sender, RoutedEventArgs e)
        {

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
