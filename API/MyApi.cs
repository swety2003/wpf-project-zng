﻿using DefaultWidgets.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project.Common;
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

        public static string ServerType = "/dev-api";

        public static string server { get; set; }
        //= "https://console-mock.apipost.cn/app/mock/project/9cce2996-185a-4aa6-9589-ef93747e1be3";

        public static Request request { get; set; }


        public static void ClearToken()
        {
            request.ClearToken();
        }

        public static void EnsuerInit()
        {
            if (request == null)
            {
                request = new Request();

            }
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


        public static void SetToken(string token)
        {
            EnsuerInit();
            request.RefreshToken(token);
        }
    }


    internal static class AdminLogin
    {




        public static async Task<string> LoginAsync(string id, string pwd)
        {
            APICOMMON.EnsuerInit();
            APICOMMON.ClearToken();
            string path = $"{APICOMMON.ServerType}/cupboard/user/login";
            var url = $"{APICOMMON.server}{path}";
            //Dictionary<string, string> data = new Dictionary<string, string>();
            Dictionary<String, String> data = new Dictionary<String, String>();


            data.Add("username", id);
            data.Add("password", pwd);


            var r = await APICOMMON.request.PostJson(url, JsonConvert.SerializeObject(data));

            return r;

        }

    }

    public static class UserLogin
    {
        public static async Task<string> LoginAsync(string id, string pwd)
        {

            APICOMMON.EnsuerInit();
            APICOMMON.ClearToken();

            string path = $"{APICOMMON.ServerType}/cupboard/user/login";
            var url = $"{APICOMMON.server}{path}";
            //Dictionary<string, string> data = new Dictionary<string, string>();

            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("username", id);
            data.Add("password", pwd);


            var r = await APICOMMON.request.PostJson(url, JsonConvert.SerializeObject(data));

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
                public ObservableCollection<RowsItem> rows { get; set; }
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

        public static async Task<string> switchList(string pageNum, string pageSize, string cabinetgroupId)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/toolswitch/list";
            var url = $"{APICOMMON.server}{path}";

            //var url1 = "https://console-mock.apipost.cn/app/mock/project/9cce2996-185a-4aa6-9589-ef93747e1be3/{APICOMMON.ServerType}/cupboard/toolswitch/list";


            //var c=url == url1;


            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("pageNum", pageNum);
            data.Add("cabinetgroupId", cabinetgroupId);
            data.Add("pageSize", pageSize);
            var r = await APICOMMON.request.Get(url, data);

            return r;

        }

        public static async Task<string> switchListAdd(string switchName, string switchIp, string switchPort, string cabinetgroupId)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/toolswitch";
            var url = $"{APICOMMON.server}{path}";

            //var url1 = "https://console-mock.apipost.cn/app/mock/project/9cce2996-185a-4aa6-9589-ef93747e1be3/{APICOMMON.ServerType}/cupboard/toolswitch/list";


            //var c=url == url1;

            Dictionary<String, String> data = new Dictionary<String, String>();


            data.Add("switchName", switchName);
            data.Add("switchIp", switchIp);
            data.Add("cabinetgroupId", cabinetgroupId);
            data.Add("switchPort", switchPort);

            //var r = await APICOMMON.request.Post(url, data);
            var r = await APICOMMON.request.PostJson(url, JsonConvert.SerializeObject(data));


            return r;

        }

        public static async Task<string> Edit(string switchName, string switchIp, string switchPort, string cabinetgroupId, string switchId)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/toolswitch";
            var url = $"{APICOMMON.server}{path}";

            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("switchId", switchId);
            data.Add("switchName", switchName);
            data.Add("switchIp", switchIp);
            data.Add("switchPort", switchPort);
            data.Add("cabinetgroupId", cabinetgroupId);

            //var r = await APICOMMON.request.Post(url, data);
            var r = await APICOMMON.request.PutJson(url, JsonConvert.SerializeObject(data));


            return r;

        }

        public static async Task<string> switchDel(string id)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/toolswitch";
            var url = $"{APICOMMON.server}{path}";

            //var url1 = "https://console-mock.apipost.cn/app/mock/project/9cce2996-185a-4aa6-9589-ef93747e1be3/{APICOMMON.ServerType}/cupboard/toolswitch/list";


            //var c=url == url1;

            Dictionary<String, String> data = new Dictionary<String, String>();
            url += "/" + id;

            //var r = await APICOMMON.request.Post(url, data);
            var r = await APICOMMON.request.Delete(url);


            return r;
        }
    }


    //柜组
    public static class CabinetGroup
    {

        public class GroupDataType
        {
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
                public int cabinetgroupId { get; set; }
                /// <summary>
                /// 柜组一
                /// </summary>
                public string cabinetgroupName { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string cabinetgroupNo { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int? warehouseId { get; set; }
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
                public ObservableCollection<RowsItem> rows { get; set; }
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

        public static async Task<string> GroupList(string pageNum, string pageSize)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/toolcabinetgroup/list";
            var url = $"{APICOMMON.server}{path}";

            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("pageNum", pageNum);
            data.Add("pageSize", pageSize);
            var r = await APICOMMON.request.Get(url, data);

            return r;

        }





    }



    //天线
    public static class antennagroup
    {
        public class GroupDataType
        {
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
                public int antennagroupId { get; set; }
                /// <summary>
                /// 天线组名称
                /// </summary>
                public string antennagroupName { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string antennagroupIp { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string antennagroupPort { get; set; }
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
                public ObservableCollection<RowsItem> rows { get; set; }
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

        public static async Task<string> GroupList(string pageNum, string pageSize, string cabinetgroupId)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/toolantennagroup/list";
            var url = $"{APICOMMON.server}{path}";



            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("pageNum", pageNum);
            data.Add("cabinetgroupId", cabinetgroupId);
            data.Add("pageSize", pageSize);
            var r = await APICOMMON.request.Get(url, data);

            return r;

        }


        public static async Task<string> Add(string antennagroupName, string antennagroupIp, string antennagroupPort, string cabinetgroupId)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/toolantennagroup";
            var url = $"{APICOMMON.server}{path}";

            Dictionary<String, String> data = new Dictionary<String, String>();


            data.Add("antennagroupName", antennagroupName);
            data.Add("antennagroupIp", antennagroupIp);
            data.Add("antennagroupPort", antennagroupPort);
            data.Add("cabinetgroupId", cabinetgroupId);

            //var r = await APICOMMON.request.Post(url, data);
            var r = await APICOMMON.request.PostJson(url, JsonConvert.SerializeObject(data));


            return r;

        }


        public static async Task<string> Del(string id)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/toolantennagroup";
            var url = $"{APICOMMON.server}{path}";


            Dictionary<String, String> data = new Dictionary<String, String>();
            url += "/" + id;

            //var r = await APICOMMON.request.Post(url, data);
            var r = await APICOMMON.request.Delete(url);


            return r;
        }

        public static async Task<string> Edit(string antennagroupName, string antennagroupIp, string antennagroupPort, string cabinetgroupId, string antennagroupId)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/toolantennagroup";
            var url = $"{APICOMMON.server}{path}";

            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("antennagroupId", antennagroupId);
            data.Add("antennagroupName", antennagroupName);
            data.Add("antennagroupIp", antennagroupIp);
            data.Add("antennagroupPort", antennagroupPort);
            data.Add("cabinetgroupId", cabinetgroupId);

            //var r = await APICOMMON.request.Post(url, data);
            var r = await APICOMMON.request.PutJson(url, JsonConvert.SerializeObject(data));


            return r;

        }



    }


    //
    public static class ToolDoor
    {


        public class DataType
        {
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
                public int doorId { get; set; }
                /// <summary>
                /// 门禁名称
                /// </summary>
                public string doorName { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string doorIp { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string doorPort { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string account { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string password { get; set; }
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


        public static async Task<string> Get(string pageNum, string pageSize, string cabinetgroupId)
        {
            string path = $"{APICOMMON.ServerType}/cupboard/tooldoor/list";
            APICOMMON.EnsuerInit();
            var url = $"{APICOMMON.server}{path}";



            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("pageNum", pageNum);
            data.Add("cabinetgroupId", cabinetgroupId);
            data.Add("pageSize", pageSize);
            var r = await APICOMMON.request.Get(url, data);

            return r;
        }


        public static async Task<string> Edit(string doorName, string doorIp,
            string doorPort, string cabinetgroupId, string doorId,
            string account, string password
            )
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/tooldoor";
            var url = $"{APICOMMON.server}{path}";

            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("doorId", doorId);
            data.Add("doorName", doorName);
            data.Add("doorIp", doorIp);
            data.Add("doorPort", doorPort);
            data.Add("account", account);
            data.Add("password", password);
            data.Add("cabinetgroupId", cabinetgroupId);

            //var r = await APICOMMON.request.Post(url, data);
            var r = await APICOMMON.request.PutJson(url, JsonConvert.SerializeObject(data));


            return r;

        }

        public static async Task<string> Add(string doorName, string doorIp,
            string doorPort, string cabinetgroupId,
            string account, string password
            )
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/tooldoor";
            var url = $"{APICOMMON.server}{path}";

            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("doorName", doorName);
            data.Add("doorIp", doorIp);
            data.Add("doorPort", doorPort);
            data.Add("account", account);
            data.Add("password", password);
            data.Add("cabinetgroupId", cabinetgroupId);

            //var r = await APICOMMON.request.Post(url, data);
            var r = await APICOMMON.request.PostJson(url, JsonConvert.SerializeObject(data));


            return r;

        }


    }


    public static class ToolGet
    {
        public class ByToolDataType
        {
            public class @params
            {
            }

            public class RowsItem : NotifyBase
            {
                //public bool selected { get;set; }

                private bool _sel;

                public bool selected
                {
                    get { return _sel; }
                    set { _sel = value; DoNotify(); }
                }


                public string image { get; set; }

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
                public int toolId { get; set; }
                /// <summary>
                /// 工具名称11111
                /// </summary>
                public string toolName { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int toolTypeId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int toolClassId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string toolNo { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int warehouseId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int cabinetgroupId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int subcabinetId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int cabinetgridId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string rfidCode { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string status { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string delFlag { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string toolIds { get; set; }
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
                public ObservableCollection<RowsItem> rows { get; set; }
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

        public static async Task<string> ByTool(string pageNum, string pageSize, string cabinetgroupId)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/login";
            var url = $"{APICOMMON.server}{APICOMMON.ServerType}/cupboard/user/toollist";



            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("pageNum", pageNum);
            data.Add("cabinetgroupId", cabinetgroupId);
            data.Add("pageSize", pageSize);
            var r = await APICOMMON.request.Get(url, data);

            return r;

        }

        public class subcabinetDT
        {
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
                public int subcabinetId { get; set; }
                /// <summary>
                /// 1#柜
                /// </summary>
                public string subcabinetName { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string subcabinetNo { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string subcabinetType { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int warehouseId { get; set; }
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

        public static async Task<string> Toolsubcabinet(string pageNum, string pageSize, string cabinetgroupId)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/toolsubcabinet/list";
            var url = $"{APICOMMON.server}{path}";


            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("pageNum", pageNum);
            data.Add("cabinetgroupId", cabinetgroupId);
            data.Add("pageSize", pageSize);
            var r = await APICOMMON.request.Get(url, data);

            return r;

        }

        public class cabinetgridDT
        {
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
                public int cabinetgridId { get; set; }
                /// <summary>
                /// 工具柜格
                /// </summary>
                public string cabinetgridName { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string cabinetgridNo { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int warehouseId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int cabinetgroupId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int subcabinetId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string antennaGroup { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string antennaNum { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string switchingValue { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string inputPoint { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string outputPoint { get; set; }
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


        public static async Task<string> ToolcabinetGrid(string pageNum, string pageSize, string subcabinetId)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/toolcabinetgrid/list";
            var url = $"{APICOMMON.server}{path}";


            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("pageNum", pageNum);
            data.Add("subcabinetId", subcabinetId);
            data.Add("pageSize", pageSize);
            var r = await APICOMMON.request.Get(url, data);

            return r;

        }



        public class UserLogDT
        {
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
                public int recorId { get; set; }
                /// <summary>
                /// 工具名称
                /// </summary>
                public string toolName { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string tooTypeId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string image { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string rfidCode { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string warehouseId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string cabinetgroupId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string position { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string operationType { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string @operator { get; set; }
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

        public static async Task<string> UserLogGet(string pageNum, string pageSize)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/toolrecord/list";
            var url = $"{APICOMMON.server}{path}";


            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("pageNum", pageNum);
            data.Add("pageSize", pageSize);
            var r = await APICOMMON.request.Get(url, data);

            return r;

        }


        public static async Task<JObject> GetGridDetailById(string id)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/toolcabinetgrid/{id}";
            var url = $"{APICOMMON.server}{path}";


            var r = await APICOMMON.request.Get(url);

            JObject o=JObject.Parse(r.ToString());

            return o;

        }


        public class AlertInfoDT
        {
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
                public int alarmId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string toolId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string alarmTool { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string alarmType { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string toolTypeId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string toolType { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string image { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string rfidCode { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int warehouseId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string warehouseName { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public int cabinetgroupId { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string cabinetgroupName { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string position { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string @operator { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string delFlag { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string status { get; set; }
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

        public static async Task<string> GetAlertInfo(string pageNum, string pageSize)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/cupboard/toolalarm/list";
            var url = $"{APICOMMON.server}{path}";


            Dictionary<String, String> data = new Dictionary<String, String>();

            data.Add("pageNum", pageNum);
            data.Add("pageSize", pageSize);
            var r = await APICOMMON.request.Get(url, data);

            return r;

        }


        public static async Task<JObject> Compare(Object compareData)
        {
            APICOMMON.EnsuerInit();
            string path = $"{APICOMMON.ServerType}/tool/matching";
            var url = $"{APICOMMON.server}{path}";

            Console.WriteLine(JsonConvert.SerializeObject(compareData));

            var r = await APICOMMON.request.PostJson(url,JsonConvert.SerializeObject(compareData));

            JObject o = JObject.Parse(r.ToString());

            return o;

        }



    }

}
