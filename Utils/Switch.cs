using HslCommunication;
using HslCommunication.ModBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WPF_Project.Utils
{
    class Switch
    {


        private string ip { get; set; }
        public string port { get; set; }


        public Switch(string ip,string port)
        {
            this.ip = ip;
            this.port = port;

        }

        // modbus开始建立连接
        private ModbusTcpNet busTcpClient = null;
        private bool IsEnable = false;
        private DispatcherTimer showTimer = new DispatcherTimer();
        public void connect()
        {
            try
            {
                if (IsEnable)
                {
                    MessageBox.Show("请勿重复建立连接!");
                    return;
                }
                string ip = this.ip;
                int port = Convert.ToInt32(this.port);
                if (ip == null || ip == "")
                {
                    MessageBox.Show("ip不能为空!");
                    return;
                }
                busTcpClient = new ModbusTcpNet(ip, port, 0x01);
                OperateResult res = busTcpClient.ConnectServer();
                if (res.IsSuccess == true) //接收状态返回值
                {
                    IsEnable = true;
                    MessageBox.Show("开启连接成功");

                    showTimer.Tick += new EventHandler(scanModBus);
                    showTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
                    showTimer.Start();
                }
                else
                {
                    MessageBox.Show("开启连接失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("开启连接失败!", ex.Message.ToString());
            }
        }

        private void scanModBus(object sender, EventArgs e)
        {
            int busLen = 16;

            try
            {
                if (!IsEnable)
                {
                    return;
                }
                if (busTcpClient == null)
                {
                    return;
                }
                string txt = "00000";
                string btnTxt = "";
                string btnITxt = "";
                for (int i = 0; i < busLen; i++)
                {
                    txt = Convert.ToString(i);
                    bool coil100 = busTcpClient.ReadCoil(txt).Content;   // 读取线圈100的通断
                    bool coilI100 = busTcpClient.ReadCoil("x=2;" + txt).Content;   // 读取线圈100的通断


                    if (i < 10)
                    {
                        btnTxt = "modBusBtnQ0" + i;
                        btnITxt = "modBusBtnI0" + i;
                    }
                    else
                    {
                        btnTxt = "modBusBtnQ" + i;
                        btnITxt = "modBusBtnI" + i;
                    }
                    //颜色指示

                    //Button modBusBtn = modBus.FindName(btnTxt) as Button;
                    //Button modBusBtnI = modBus.FindName(btnITxt) as Button;

                    //if (coil100 == true)
                    //{
                    //    modBusBtn.Background = System.Windows.Media.Brushes.Red;
                    //}
                    //else
                    //{
                    //    modBusBtn.Background = System.Windows.Media.Brushes.Black;
                    //}
                    //if (coilI100 == true)
                    //{
                    //    modBusBtnI.Background = System.Windows.Media.Brushes.Red;
                    //}
                    //else
                    //{
                    //    modBusBtnI.Background = System.Windows.Media.Brushes.Black;
                    //}
                }


            }
            catch (Exception ex)
            {
                return;
            }


        }



        public void DisConnect()
        {
            try
            {
                if (!IsEnable)
                {
                    MessageBox.Show("尚未建立连接!");
                    return;
                }
                busTcpClient.ConnectClose();
                IsEnable = false;
                MessageBox.Show("关闭连接成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("关闭连接失败!", ex.Message.ToString());
            }
        }


        public void Open(string txt)
        {
            Console.WriteLine(txt);
            try
            {
                if (!IsEnable)
                {
                    MessageBox.Show("尚未建立连接!");
                    return;
                }
                if (busTcpClient == null)
                {
                    MessageBox.Show("尚未初始化对象!");
                    return;
                }

                bool coil100 = busTcpClient.ReadCoil(txt).Content;   // 读取线圈通断

                if (coil100 == true)
                {
                    busTcpClient.WriteCoil(txt, false);// 写入线圈断
                }
                else
                {
                    busTcpClient.WriteCoil(txt, true);// 写入线圈通
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }














    }
}
