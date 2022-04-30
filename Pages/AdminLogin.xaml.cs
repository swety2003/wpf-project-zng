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
using WPF_Project.Common;
using WPF_Project.Dialogs;
using WpfWidgetDesktop.Utils;

namespace WPF_Project.Pages
{
    /// <summary>
    /// AdminLogin.xaml 的交互逻辑
    /// </summary>
    public partial class AdminLogin : Page
    {
        private class CFG
        {
            public string ip { get; set; }
            public string port { get; set; }
        }
        private string GUID = "core.admin";
        private CFG cfg;
        public AdminLogin()
        {
            InitializeComponent();

        }


        private void SetCFG()
        {

            var d = new SimpleInput();
            var a = d.ShowDialog();

            if (a == true)
            {
                MessageBox.Show(d.Result[0]+d.Result[1]);
                cfg = new CFG();
                cfg.ip = d.Result[0];
                cfg.port = d.Result[1];

                SettingProvider.Set(GUID, cfg);
            }
            else
            {
                return;
            }
        }

        private void CheckCfg()
        {
            try
            {
                cfg = JsonConvert.DeserializeObject<CFG>(SettingProvider.Get(GUID));

                if (cfg == null)
                {
                    SetCFG();
                }
            }
            catch (Exception ex)
            {
                SetCFG();
            }
        }

        public async Task TryLoginAsync()
        {

            CheckCfg();
            var id = "admin";
            var r = await API.AdminLogin.LoginAsync(id, passwdBox.Password);
            try
            {

                JObject o = JObject.Parse(r);

                int code = (int)o["code"];

                if (code == 200)
                {
                    StaticValues.MainWindow.NagivateTo(new AdminView());

                }
                else
                {

                    new ToolGetDialog1("系统提示", "密码不正确！").ShowDialog();
                    passwdBox.Password = "";
                }
            }catch (Exception ex)
            {
                MessageBox.Show("网络错误/服务端错误!");
            }





        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TryLoginAsync();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StaticValues.MainWindow.SetTitle("系统维护");
            StaticValues.Toparea.Visibility = Visibility.Visible;


            CheckCfg();
        }
    }
}
