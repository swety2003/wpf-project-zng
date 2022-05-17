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
    public class Switch
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

        // 登录并开门 输入点inputTxt 00 01 02 03 04 05...
        public void connectAndOpenDoor(string inputTxt)
        {
            try
            {
                if (IsEnable)
                {
                    Open(inputTxt);
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
                    Open(inputTxt);

                }
                else
                {
                    MessageBox.Show("开启连接失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("开启连接失败!", ex.Message.ToString());
                throw new Exception("无法开启连接！");
            }
        }

        // 检查门是否正常开启 输出点outputTxt 00 01 02 03 04 05...
        public bool checkDoorStatus(string outputTxt)
        {
            bool coilI100 = busTcpClient.ReadCoil("x=2;" + outputTxt).Content;
            return coilI100;
        }

        // 断开连接
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
            //Console.WriteLine(txt);
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
