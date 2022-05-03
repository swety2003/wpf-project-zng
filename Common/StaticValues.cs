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

            JObject o = JObject.Parse(r);

            string cabinetgroupId = (string)o["cabinetgroupId"];

            return cabinetgroupId;
        }
    }
}
