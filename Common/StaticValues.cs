using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfWidgetDesktop.Utils;

namespace WPF_Project.Common
{
    internal static class StaticValues
    {
        public static MainWindow MainWindow { get; set; }

        public static Grid Toparea { get; set; }

        public static Button scanBtn { get; set; }


        public static string TryGetCabID()
        {
            var r = SettingProvider.Get("core.paramcfg");

            try
            {

                JObject o = JObject.Parse(r);

                string cabinetgroupId = (string)o["cabinetgroupId"];

                return cabinetgroupId;

            }
            catch
            {
                return "";
            }
        }

        public static bool TryGetSysType()
        {
            try
            {

                var r = SettingProvider.Get("core.paramcfg");

                JObject o = JObject.Parse(r);

                bool IsKey = (bool)o["IsKey"];

                if (IsKey == null)
                {
                    IsKey = false;
                }
            }
            catch(Exception ex)
            {

            }
            return false;
        }
    }
}
