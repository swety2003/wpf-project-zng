﻿using Newtonsoft.Json;
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
using WPF_Project.API;
using WPF_Project.Common;
using WPF_Project.Dialogs;
using WPF_Project.Pages;
using WPF_Project.Utils;
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
        private bool SystemType { get; set; }
        public MainWindow()
        {

            InitializeComponent();
            StaticValues.MainWindow = this;
            StaticValues.Toparea = topBar;
            StaticValues.scanBtn = scanBtn;

            SystemType=StaticValues.TryGetSysType();





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


        public void ShowHomeBtn()
        {
            LoginSuccessBtn.Visibility = Visibility.Visible;
            loginLabel.Visibility = Visibility.Collapsed;
        }

        public void HideHomeBtn()
        {
            LoginSuccessBtn.Visibility = Visibility.Collapsed;
            loginLabel.Visibility = Visibility.Visible;
            if (IsLogin)
            {
                SetUserName("欢迎您，张三");

            }
        }

        public void SetUserName(string nm)
        {
            loginLabel.Content = nm;
            loginLabel.FontSize = 30;
            loginLabel.Foreground =new SolidColorBrush( Color.FromRgb(255, 255, 255));
        }

        public void SetTitle(string title)
        {
            titleTB.Text = title;

        }
        public void NavigateTo(Page p,bool ensuerLogin=true)
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

            if (SystemType)
            {
                ChangeBackGround(@"Assets\Key\KeyBg.png");
                NavigateTo(new Pages.KeyPages.Home(), false);
            }
            else
            {
                NavigateTo(new HomePageView(), false);
            }
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
#if !DEBUG
            if (!IsLogin)
            {
                API.APICOMMON.SetToken("eyJhbGciOiJIUzUxMiJ9.eyJsb2dpbl91c2VyX2tleSI6IjEzZjQ3M2VlLTRhM2MtNDc4NS1hMGYxLTM1NDgwNjcxYjc1NiJ9.lQsNM3C4EwYAwJdq0P4HbwrkCjp5Iv_Em7ouKnWw3WJ8O-7jEM4hBz2hgYoE4LQoeNDhKauqvMYUjvZvBOvvZQ");
                IsLogin = true;
            }
#else

            if (!IsLogin)
            {

                //new ToolGetDialog1("系统提示","请刷卡或扫脸登录").ShowDialog();


                var d = new Dialogs.UserLogin();
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

                            //StaticValues.MainWindow.LoginSuccess();

                            API.APICOMMON.SetToken(token);


                            var r = new CustomDialog("系统提示", "欢迎你，张三\n 是否快速取还？").ShowDialog();
                            if (r == true)
                            {
                                StaticValues.MainWindow.NavigateTo(new Pages.ToolGet());

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


        DoorAcc dooracc;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.cabid = StaticValues.TryGetCabID();

            if (tooldoor != null)
            {
                this.dooracc = new DoorAcc(tooldoor.doorIp, tooldoor.doorPort, tooldoor.account, tooldoor.password);

                dooracc.Init();

                dooracc.doorDeploy();

            }




            if (SystemType)
            {
                ChangeBackGround(@"Assets\Key\KeyBg.png");
                NavigateTo(new Pages.KeyPages.Home(),false);
            }
            else
            {
                NavigateTo(new HomePageView(),false);
            }



        }


        private string cabid;
        private ToolDoor.DataType.RowsItem tooldoor;
        private async Task TryGetDoorInfoAsync()
        {
            if (string.IsNullOrEmpty(cabid))
            {
                return;
            }
            var r = await ToolDoor.Get("1", "10", cabid);
            var origin = JsonConvert.DeserializeObject<ToolDoor.DataType.Root>(r);


            this.tooldoor = new ToolDoor.DataType.RowsItem();
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
