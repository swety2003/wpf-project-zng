using DefaultWidgets.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfWidgetDesktop.Utils;

namespace WPF_Project.API
{

    public class APICFG
    {
        public string ip { get; set; }
        public string port { get; set; }
    }

    public static class APICOMMON
    {
        private static string GUID = "core.admin";

        public static string server { get; set; } = "https://console-mock.apipost.cn/app/mock/project/9cce2996-185a-4aa6-9589-ef93747e1be3";

        public static Request request { get; set; }

        public static void EnsuerInit()
        {
            request = new Request();
            if (server == null)
            {

                APICFG apicfg = new APICFG();
                var cfg = SettingProvider.Get(GUID);
                apicfg = JsonConvert.DeserializeObject<APICFG>(SettingProvider.Get(GUID));

                if (cfg != null)
                {
                    server = $"http://{apicfg.ip}:{apicfg.port}";
                }
            }
        }
    }


    internal static class AdminLogin
    {


        

        public static async Task<string> LoginAsync(string id,string pwd)
        {
            APICOMMON.EnsuerInit();
            string path = "";
            var url = $"{APICOMMON.server}/dev-api/login";
            //Dictionary<string, string> data = new Dictionary<string, string>();

            List<KeyValuePair<String, String>> data=new List<KeyValuePair<String, String>>();

            data.Add(new KeyValuePair<string, string>("username", id));
            data.Add(new KeyValuePair<string, string>("password", pwd));


            var r =await APICOMMON.request.Post(url, data);

            return r;

        }

    }


    public static class Switch
    {
        public class switchListDataType
        {
            //如果好用，请收藏地址，帮忙分享。
            public class @params
            {
            }

            public class RowsItem
            {
                /// <summary>
                /// 
                /// </summary>
                public string searchValue { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string createBy { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string createTime { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string updateBy { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string updateTime { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string remark { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public @params @params { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int switchId { get; set; }
                /// <summary>
                /// 开关量名称
                /// </summary>
                public string switchName { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string switchIp { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string switchPort { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int cabinetgroupId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string delFlag { get; set; }
            }

            public class Root
            {
                /// <summary>
                /// 
                /// </summary>
                public int total { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public List<RowsItem> rows { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int code { get; set; }
                /// <summary>
                /// 查询成功
                /// </summary>
                public string msg { get; set; }
            }

        }

        public static async Task<string> switchList(string pageNum, string pageSize,string cabinetgroupId)
        {
            APICOMMON.EnsuerInit();
            string path = "/dev-api/login";
            var url = $"{APICOMMON.server}/dev-api/cupboard/toolswitch/list";

            //var url1 = "https://console-mock.apipost.cn/app/mock/project/9cce2996-185a-4aa6-9589-ef93747e1be3//dev-api/cupboard/toolswitch/list";


            //var c=url == url1;


            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("pageNum", pageNum);
            data.Add("cabinetgroupId", cabinetgroupId);
            data.Add("pageSize", pageSize);


            var r = await APICOMMON.request.Get(url, data);

            return r;

        }

    }

    public static class CabinetGroup
    {

    }

}
