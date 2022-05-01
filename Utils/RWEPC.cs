using IntellectTestTool.UHFReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using UHF;

namespace WPF_Project.Utils
{
    internal class RWEPC
    {

        #region param set
        //读写器
        private byte fComAdr = 0xff; //当前操作的ComAdr
        private int ferrorcode;
        private byte fBaud;
        private double fdminfre;
        private double fdmaxfre;
        private int fCmdRet = 30; //所有执行指令的返回值
        private bool fisinventoryscan_6B;
        private byte[] fOperEPC = new byte[100];
        private byte[] fPassWord = new byte[4];
        private byte[] fOperID_6B = new byte[10];
        ArrayList list = new ArrayList();
        private List<string> epclist = new List<string>();
        private List<string> tidlist = new List<string>();
        private int CardNum1 = 0;
        private string fInventory_EPC_List; //存贮询查列表（如果读取的数据没有变化，则不进行刷新）
        private int frmcomportindex;
        private bool SeriaATflag = false;
        private byte Target = 0;
        private byte InAnt = 0;
        private byte Scantime = 0;
        private byte FastFlag = 0;
        private byte Qvalue = 0;
        private byte Session = 0;
        private int total_turns = 0;//轮数
        private int total_tagnum = 0;//标签数量
        private int CardNum = 0;
        private int total_time = 0;//总时间
        private int targettimes = 0;
        private byte TIDFlag = 0;
        private byte tidLen = 0;
        private byte tidAddr = 0;
        public static byte antinfo = 0;
        private int AA_times = 0;
        private int CommunicationTime = 0;
        RFIDCallBack elegateRFIDCallBack;

        private static List<DeviceClass> DevList;
        private static SearchCallBack searchCallBack = new SearchCallBack(searchCB);
        private string ReadTypes = "";

        private ObservableCollection<EpcInfo> epcInfo;
        private List<EpcInfo> epcInfoList = new List<EpcInfo>();

        private static void searchCB(IntPtr dev, IntPtr data)
        {
            uint ipAddr = 0;
            StringBuilder devname = new StringBuilder(100);
            StringBuilder macAdd = new StringBuilder(100);
            //获取搜索到的设备信息；
            DevControl.tagErrorCode eCode = DevControl.DM_GetDeviceInfo(dev, ref ipAddr, macAdd, devname);
            if (eCode == DevControl.tagErrorCode.DM_ERR_OK)
            {
                //将搜索到的设备加入设备列表；
                DeviceClass device = new DeviceClass(dev, ipAddr, macAdd.ToString(), devname.ToString());
                DevList.Add(device);
            }
            else
            {
                //异常处理；
                string errMsg = ErrorHandling.GetErrorMsg(eCode);
            }

        }



        #endregion





        private bool[] antennaList = new bool[16];


        public async Task<List<EpcInfo>> GetEpcInfo()
        {


            


            return epcInfoList;
        }

        private string ip { get; set; }
        public string port { get; set; }


        private bool _connected;

        public bool Connected
        {
            get { return _connected; }
            set { 
                _connected = value; 

                if (value == true)
                {
                    cardStartQuery();
                }
            
            
            
            }
        }

        public RWEPC(string ip, string port)
        {
            this.ip=ip;
            this.port=  port;


            //if (CHCNetSDK.NET_DVR_Init() == false)
            //{
            //    MessageBox.Show("NET_DVR_Init error!");
            //    return;
            //}
            //CHCNetSDK.NET_DVR_SetLogToFile(3, "./", false);

            //读写器
            //初始化设备列表；
            DevList = new List<DeviceClass>();

            epcInfo = new ObservableCollection<EpcInfo> { };
            //cardQueryListView.ItemsSource = epcInfo;

            //初始化设备控制模块；
            DevControl.tagErrorCode eCode = DevControl.DM_Init(searchCallBack, IntPtr.Zero);
            if (eCode != DevControl.tagErrorCode.DM_ERR_OK)
            {
                //如果初始化失败则关闭程序，并进行异常处理；
                string errMsg = ErrorHandling.HandleError(eCode);
                throw new Exception(errMsg);
            }
            elegateRFIDCallBack = new RFIDCallBack(GetUid);

        }

        public void GetUid(IntPtr p, Int32 nEvt)
        {

            RFIDTag ce = (RFIDTag)Marshal.PtrToStructure(p, typeof(RFIDTag));



                    int Antnum = ce.ANT;
                    string str_ant = Convert.ToString(1 << Antnum, 2).PadLeft(16, '0');
                    string epclen = Convert.ToString(ce.LEN, 16);
                    if (epclen.Length == 1) epclen = "0" + epclen;
                    string para = str_ant + "," + epclen + ce.UID + "," + ce.RSSI.ToString() + " ";

                    //检测是否已存在

                    var resultIndex = epcInfoList.FindIndex(t => t.epc == epclen + ce.UID);
                    int epcLen = epcInfoList.Count;
                    if (resultIndex != -1)
                    {
                        epcInfo[resultIndex].num++;
                    }
                    else
                    {
                        int no = ++epcLen;
                        epcInfoList.Add(new EpcInfo((no).ToString(), epclen + ce.UID, 1, ce.RSSI.ToString(), str_ant));
                        epcInfo.Add(new EpcInfo((no).ToString(), epclen + ce.UID, 1, ce.RSSI.ToString(), str_ant));
                    }

                    //下面开始绑定数据,固定的两行代码
                    //cardQueryListView.DataContext = epcInfoList;
                    //cardQueryListView.SetBinding(ListView.ItemsSourceProperty, new Binding());

                    //0000000000100000,0cE2801170000002130B44D343,81 
                    //Console.WriteLine(para);

                    total_tagnum++;
                    CardNum++;



        }


        private string GetReturnCodeDesc(int cmdRet)
        {
            switch (cmdRet)
            {
                case 0x00:
                case 0x26:
                    return "操作成功";
                case 0x01:
                    return "询查时间结束前返回";
                case 0x02:
                    return "指定的询查时间溢出";
                case 0x03:
                    return "本条消息之后，还有消息";
                case 0x04:
                    return "读写模块存储空间已满";
                case 0x05:
                    return "访问密码错误";
                case 0x09:
                    return "销毁密码错误";
                case 0x0a:
                    return "销毁密码不能为全0";
                case 0x0b:
                    return "电子标签不支持该命令";
                case 0x0c:
                    return "对该命令，访问密码不能为全0";
                case 0x0d:
                    return "电子标签已经被设置了读保护，不能再次设置";
                case 0x0e:
                    return "电子标签没有被设置读保护，不需要解锁";
                case 0x10:
                    return "有字节空间被锁定，写入失败";
                case 0x11:
                    return "不能锁定";
                case 0x12:
                    return "已经锁定，不能再次锁定";
                case 0x13:
                    return "参数保存失败,但设置的值在读写模块断电前有效";
                case 0x14:
                    return "无法调整";
                case 0x15:
                    return "询查时间结束前返回";
                case 0x16:
                    return "指定的询查时间溢出";
                case 0x17:
                    return "本条消息之后，还有消息";
                case 0x18:
                    return "读写模块存储空间已满";
                case 0x19:
                    return "电子不支持该命令或者访问密码不能为0";
                case 0x1A:
                    return "标签自定义功能执行错误";
                case 0xF8:
                    return "检测天线错误";
                case 0xF9:
                    return "命令执行出错";
                case 0xFA:
                    return "有电子标签，但通信不畅，无法操作";
                case 0xFB:
                    return "无电子标签可操作";
                case 0xFC:
                    return "电子标签返回错误代码";
                case 0xFD:
                    return "命令长度错误";
                case 0xFE:
                    return "不合法的命令";
                case 0xFF:
                    return "参数错误";
                case 0x30:
                    return "通讯错误";
                case 0x31:
                    return "CRC校验错误";
                case 0x32:
                    return "返回数据长度有错误";
                case 0x33:
                    return "通讯繁忙，设备正在执行其他指令";
                case 0x34:
                    return "繁忙，指令正在执行";
                case 0x35:
                    return "端口已打开";
                case 0x36:
                    return "端口已关闭";
                case 0x37:
                    return "无效句柄";
                case 0x38:
                    return "无效端口";
                case 0xEE:
                    return "命令代码错误";
                default:
                    return Convert.ToString(cmdRet, 16);
            }
        }

        //读写器信息
        private void btGetInformation_Click()
        {
            byte TrType = 0;
            byte[] VersionInfo = new byte[2];
            byte ReaderType = 0;
            byte ScanTime = 0;
            byte dmaxfre = 0;
            byte dminfre = 0;
            byte powerdBm = 0;
            byte FreBand = 0;
            byte AntCfg0 = 0;
            byte BeepEn = 0;
            byte AntCfg1 = 0;
            byte CheckAnt = 0;

            int ctime = System.Environment.TickCount;
            fCmdRet = RWDev.GetReaderInformation(ref fComAdr, VersionInfo, ref ReaderType, ref TrType, ref dmaxfre, ref dminfre, ref powerdBm, ref ScanTime, ref AntCfg0, ref BeepEn, ref AntCfg1, ref CheckAnt, frmcomportindex);
            if (fCmdRet != 0)
            {
                string strLog = "获取读写器信息失败，原因： " + GetReturnCodeDesc(fCmdRet);
                MessageBox.Show(strLog);
            }
            else
            {
                CommunicationTime = System.Environment.TickCount - ctime;
                //comboBox_maxtime.SelectedIndex = ScanTime;
                //ReaderType = 0x20;
                switch (ReaderType)
                {
                    case 0x27:
                        {
                            ReadTypes = "27";
                            //天线显示

                            //text_RDVersion.Text = "UHFReaderUR6286--" + Convert.ToString(VersionInfo[0], 10).PadLeft(2, '0') + "." + Convert.ToString(VersionInfo[1], 10).PadLeft(2, '0');
                        }
                        break;
                    default:
                        {
                            ReadTypes = "27";
                            //text_RDVersion.Text = "UHFReader6286--" + Convert.ToString(VersionInfo[0], 10).PadLeft(2, '0') + "." + Convert.ToString(VersionInfo[1], 10).PadLeft(2, '0');
                        }
                        break;

                }
                //ComboBox_PowerDbm.SelectedIndex = Convert.ToInt32(powerdBm);
                if (powerdBm == 255)
                {
                    //btGetRfPower_Click(null, null);
                }
                else
                {
                    //ComboBox_PowerDbm.SelectedIndex = Convert.ToInt32(powerdBm);
                }
                //text_address.Text = Convert.ToString(fComAdr, 16).PadLeft(2, '0');
                //射频频谱
                FreBand = Convert.ToByte(((dmaxfre & 0xc0) >> 4) | (dminfre >> 6));
                switch (FreBand)
                {
                    case 1:
                        {
                            //radioButton_band1.Checked = true;
                            fdminfre = 920.125 + (dminfre & 0x3F) * 0.25;
                            fdmaxfre = 920.125 + (dmaxfre & 0x3F) * 0.25;
                        }
                        break;
                    case 2:
                        {
                            //radioButton_band2.Checked = true;
                            fdminfre = 902.75 + (dminfre & 0x3F) * 0.5;
                            fdmaxfre = 902.75 + (dmaxfre & 0x3F) * 0.5;
                        }
                        break;
                    case 3:
                        {
                            //radioButton_band3.Checked = true;
                            fdminfre = 917.1 + (dminfre & 0x3F) * 0.2;
                            fdmaxfre = 917.1 + (dmaxfre & 0x3F) * 0.2;
                        }
                        break;
                    case 4:
                        {
                            //radioButton_band4.Checked = true;
                            fdminfre = 865.1 + (dminfre & 0x3F) * 0.2;
                            fdmaxfre = 865.1 + (dmaxfre & 0x3F) * 0.2;
                        }
                        break;
                    case 8:
                        {
                            //radioButton_band8.Checked = true;
                            fdminfre = 840.125 + (dminfre & 0x3F) * 0.25;
                            fdmaxfre = 840.125 + (dmaxfre & 0x3F) * 0.25;
                        }
                        break;
                    case 12:
                        {
                            //radioButton_band12.Checked = true;
                            fdminfre = 902 + (dminfre & 0x3F) * 0.5;
                            fdmaxfre = 902 + (dmaxfre & 0x3F) * 0.5;
                        }
                        break;
                }
                //单频点
                //if (fdmaxfre != fdminfre)
                //    CheckBox_SameFre.Checked = false;
                //ComboBox_dminfre.SelectedIndex = dminfre & 0x3F;
                //ComboBox_dmaxfre.SelectedIndex = dmaxfre & 0x3F;

                //蜂鸣器
                //switch (BeepEn)
                //{
                //    case 1:
                //        Radio_beepEn.Checked = true;
                //        break;
                //    case 0:
                //        Radio_beepDis.Checked = true;
                //        break;
                //}





                #region 天线显示
                if ((AntCfg0 & 0x01) == 0x01)
                {
                    antennaList[0] = true;
                }
                else
                {
                    antennaList[0] = false;
                }

                if ((AntCfg0 & 0x02) == 0x02)
                {
                    antennaList[1] = true;
                }
                else
                {
                    antennaList[1] = false;
                }

                if ((AntCfg0 & 0x04) == 0x04)
                {
                    antennaList[2] = true;
                }
                else
                {
                    antennaList[2] = false;
                }

                if ((AntCfg0 & 0x08) == 0x08)
                {
                    antennaList[3] = true;
                }
                else
                {
                    antennaList[3] = false;
                }

                if ((AntCfg0 & 0x10) == 0x10)
                {
                    antennaList[4] = true;
                }
                else
                {
                    antennaList[4] = false;
                }

                if ((AntCfg0 & 0x20) == 0x20)
                {
                    antennaList[5] = true;
                }
                else
                {
                    antennaList[5] = false;
                }

                if ((AntCfg0 & 0x40) == 0x40)
                {
                    antennaList[6] = true;
                }
                else
                {
                    antennaList[6] = false;
                }

                if ((AntCfg0 & 0x80) == 0x80)
                {
                    antennaList[7] = true;
                }
                else
                {
                    antennaList[7] = false;
                }



                if ((AntCfg1 & 0x01) == 0x01)
                {
                    antennaList[8] = true;
                }
                else
                {
                    antennaList[8] = false;
                }

                if ((AntCfg1 & 0x02) == 0x02)
                {
                    antennaList[9] = true;
                }
                else
                {
                    antennaList[9] = false;
                }

                if ((AntCfg1 & 0x04) == 0x04)
                {
                    antennaList[10] = true;
                }
                else
                {
                    antennaList[10] = false;
                }

                if ((AntCfg1 & 0x08) == 0x08)
                {
                    antennaList[11] = true;
                }
                else
                {
                    antennaList[11] = false;
                }

                if ((AntCfg1 & 0x10) == 0x10)
                {
                    antennaList[12] = true;
                }
                else
                {
                    antennaList[12] = false;
                }

                if ((AntCfg1 & 0x20) == 0x20)
                {
                    antennaList[13] = true;
                }
                else
                {
                    antennaList[13] = false;
                }

                if ((AntCfg1 & 0x40) == 0x40)
                {
                    antennaList[14] = true;
                }
                else
                {
                    antennaList[14] = false;
                }

                if ((AntCfg1 & 0x80) == 0x80)
                {
                    antennaList[15] = true;
                }
                else
                {
                    antennaList[15] = false;
                }

                #endregion
                //天线检测
                //if (CheckAnt == 0)
                //{

                //}
                //else
                //{

                //}
                //string strLog = "获取读写器信息成功 ";
            }
        }


        public void Connect()
        {
            try
            {
                string strException = string.Empty;
                string ipAddress = ip;
                int nPort = Convert.ToInt32(port);
                fComAdr = 255;
                int FrmPortIndex = 0;
                fCmdRet = RWDev.OpenNetPort(nPort, ipAddress, ref fComAdr, ref FrmPortIndex);
                if (fCmdRet != 0)
                {
                    string strLog = "连接读写器失败，失败原因： " + GetReturnCodeDesc(fCmdRet);
                    MessageBox.Show(strLog);
                    return;
                }
                else
                {
                    frmcomportindex = FrmPortIndex;
                    string strLog = "连接读写器 " + ipAddress + "@" + nPort.ToString();
                    MessageBox.Show(strLog);

                    //Connected=true;
                }
                //cardStart.IsEnabled = false;
                //cardStop.IsEnabled = true;
                btGetInformation_Click();//获取读写器信息
                if (FrmPortIndex > 0)
                {
                    RWDev.InitRFIDCallBack(elegateRFIDCallBack, true, FrmPortIndex);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DisConnect()
        {
            if (frmcomportindex > 0)
                fCmdRet = RWDev.CloseNetPort(frmcomportindex);
            if (fCmdRet == 0)
            {
                frmcomportindex = -1;
                //cardStart.IsEnabled = true;
                //cardStop.IsEnabled = false;
            }
            if (fCmdRet != 0)
            {
                string strLog = "TCP关闭不成功： " + GetReturnCodeDesc(fCmdRet);
                MessageBox.Show(strLog);
                return;
            }
            else
            {
                string strLog = "TCP关闭成功";
                MessageBox.Show(strLog);
            }

        }


        byte[] antlist = new byte[16];
        private volatile bool fIsInventoryScan = false;
        private volatile bool toStopThread = false;
        private Thread mythread = null;
        public void cardStartQuery()
        {
            //检测是否连接
            //if (cardStart.IsEnabled == true)
            //{
            //    MessageBox.Show("请先连接");
            //    return;
            //}

            epclist.Clear();
            tidlist.Clear();

            //comboBox_EPC.Items.Clear();
            //text_epc.Text = "";

            AA_times = 0;
            try
            {
                Scantime = Convert.ToByte(2000);
            }
            catch (OverflowException)
            {

            }


            //返回速率
            Qvalue = Convert.ToByte(4);
            //if (checkBox_rate.Checked)
            //    Qvalue = Convert.ToByte(com_Q.SelectedIndex | 0x80);
            //else
            //    Qvalue = Convert.ToByte(4);


            Session = Convert.ToByte(0);
            if (Session == 4)
                Session = 255;

            //epc查询
            //TIDFlag = 0;
            //tid查询
            TIDFlag = 1;
            tidAddr = (byte)(Convert.ToInt32("2", 16) & 0x00FF);
            tidLen = Convert.ToByte("4", 16);

            total_turns = 0;
            total_tagnum = 0;
            //连续5次无卡A/B切换
            targettimes = Convert.ToInt32(5);
            total_time = System.Environment.TickCount;
            fIsInventoryScan = false;

            //cardStartQuery.IsEnabled = false;

            Array.Clear(antlist, 0, 16);

            #region 天线显示
            if (antennaList[0] == true)
            {
                antlist[0] = 1;
                InAnt = 0x80;
            }
            if (antennaList[1] == true)
            {
                antlist[1] = 1;
                InAnt = 0x81;
            }
            if (antennaList[2] == true)
            {
                antlist[2] = 1;
                InAnt = 0x82;
            }
            if (antennaList[3] == true)
            {
                antlist[3] = 1;
                InAnt = 0x83;
            }


            if (antennaList[4] == true)
            {
                antlist[4] = 1;
                InAnt = 0x84;
            }

            if (antennaList[5] == true)
            {
                antlist[5] = 1;
                InAnt = 0x85;
            }

            //if (true)
            //{
            //    antlist[5] = 1;
            //    InAnt = 0x85;
            //}


            if (antennaList[6] == true)
            {
                antlist[6] = 1;
                InAnt = 0x86;
            }

            if (antennaList[7] == true)
            {
                antlist[7] = 1;
                InAnt = 0x87;
            }

            if (antennaList[8] == true)
            {
                antlist[8] = 1;
                InAnt = 0x88;
            }
            if (antennaList[9] == true)
            {
                antlist[9] = 1;
                InAnt = 0x89;
            }
            if (antennaList[10] == true)
            {
                antlist[10] = 1;
                InAnt = 0x8A;
            }
            if (antennaList[11] == true)
            {
                antlist[11] = 1;
                InAnt = 0x8B;
            }


            if (antennaList[12] == true)
            {
                antlist[12] = 1;
                InAnt = 0x8C;
            }

            if (antennaList[13] == true)
            {
                antlist[13] = 1;
                InAnt = 0x8D;
            }

            if (antennaList[14] == true)
            {
                antlist[14] = 1;
                InAnt = 0x8E;
            }

            if (antennaList[15] == true)
            {
                antlist[15] = 1;
                InAnt = 0x8F;
            }
            #endregion


            Target = (byte)0;
            toStopThread = false;
            if (fIsInventoryScan == false)
            {
                mythread = new Thread(new ThreadStart(inventory));
                mythread.IsBackground = true;
                mythread.Start();
                //timer_answer.Enabled = true;

                //inventory();

            }


        }

        public void EndScan()
        {
            toStopThread = true;
            //cardStartQuery.IsEnabled = true;
        }

        private void inventory()
        {
            fIsInventoryScan = true;
            while (!toStopThread)
            {
                if (Session == 255)
                {
                    FastFlag = 0;
                    flash_G2();

                }
                else
                {
                    for (int m = 0; m < 16; m++)
                    {
                        switch (m)
                        {
                            case 0:
                                InAnt = 0x80;
                                break;
                            case 1:
                                InAnt = 0x81;
                                break;
                            case 2:
                                InAnt = 0x82;
                                break;
                            case 3:
                                InAnt = 0x83;
                                break;
                            case 4:
                                InAnt = 0x84;
                                break;
                            case 5:
                                InAnt = 0x85;
                                break;
                            case 6:
                                InAnt = 0x86;
                                break;
                            case 7:
                                InAnt = 0x87;
                                break;


                            case 8:
                                InAnt = 0x88;
                                break;
                            case 9:
                                InAnt = 0x89;
                                break;
                            case 10:
                                InAnt = 0x8A;
                                break;
                            case 11:
                                InAnt = 0x8B;
                                break;
                            case 12:
                                InAnt = 0x8C;
                                break;
                            case 13:
                                InAnt = 0x8D;
                                break;
                            case 14:
                                InAnt = 0x8E;
                                break;
                            case 15:
                                InAnt = 0x8F;
                                break;
                        }
                        FastFlag = 1;

                        if (antlist[m] == 1)
                        {

                            if (Session > 1)//s2,s3
                            {
                                if ((AA_times + 1 > targettimes))
                                {
                                    Target = Convert.ToByte(1 - Target);  //如果连续2次未读到卡片，A/B状态切换。
                                    AA_times = 0;
                                }
                            }
                            flash_G2();
                        }
                    }
                }
                Thread.Sleep(5);
            }


                    if (fIsInventoryScan)
                    {
                        toStopThread = true;//标志，接收数据线程判断stop为true，正常情况下会自动退出线程           
                        mythread.Abort();//若线程无法退出，强制结束
                        fIsInventoryScan = false;
                    }
                    fIsInventoryScan = false;

                    //cardStartQuery.IsEnabled = true;


        }



        private void flash_G2()
        {
            byte Ant = 0;
            int TagNum = 0;
            int Totallen = 0;
            int EPClen, m;
            byte[] EPC = new byte[50000];
            int CardIndex;
            string temps, temp;
            temp = "";
            string sEPC;
            byte MaskMem = 0;
            byte[] MaskAdr = new byte[2];
            byte MaskLen = 0;
            byte[] MaskData = new byte[100];
            byte MaskFlag = 0;
            MaskFlag = 0;
            int cbtime = System.Environment.TickCount;
            CardNum = 0;

            string para = "";
            fCmdRet = RWDev.Inventory_G2(ref fComAdr, Qvalue, Session, MaskMem, MaskAdr, MaskLen, MaskData, MaskFlag, tidAddr, tidLen, TIDFlag, Target, InAnt, Scantime, FastFlag, EPC, ref Ant, ref Totallen, ref TagNum, frmcomportindex);
            int cmdTime = System.Environment.TickCount - cbtime;//命令时间
            if ((fCmdRet != 0x01) && (fCmdRet != 0x02) && (fCmdRet != 0xF8) && (fCmdRet != 0xF9) && (fCmdRet != 0xEE) && (fCmdRet != 0xFF) && (fCmdRet != 0xFE))
            {
                if (true)
                {
                    if ((frmcomportindex > 0) && (frmcomportindex < 256))
                    {
                        fCmdRet = RWDev.CloseNetPort(frmcomportindex);
                        if (fCmdRet == 0) frmcomportindex = -1;
                        Thread.Sleep(1000);
                    }
                    fComAdr = 255;
                    string ipAddress = ip;
                    int nPort = Convert.ToInt32(port);
                    fCmdRet = RWDev.OpenNetPort(nPort, ipAddress, ref fComAdr, ref frmcomportindex);
                    if (fCmdRet == 0)
                    {
                        if (frmcomportindex > 0)
                            RWDev.InitRFIDCallBack(elegateRFIDCallBack, true, frmcomportindex);
                    }
                }
            }
            if (fCmdRet == 0x30)
            {
                CardNum = 0;
            }
            if (CardNum == 0)
            {
                if (Session > 1)
                    AA_times = AA_times + 1;//没有得到标签只更新状态栏
            }
            else
                AA_times = 0;

            if ((fCmdRet == 1) || (fCmdRet == 2) || (fCmdRet == 0xFB) || (fCmdRet == 0x26))
            {
                if (cmdTime > CommunicationTime)
                    cmdTime = cmdTime - CommunicationTime;//减去通讯时间等于标签的实际时间

                if (cmdTime > 0)
                {
                    int tagrate = (CardNum * 1000) / cmdTime;//速度等于张数/时间

                    para = tagrate.ToString() + "," + total_tagnum.ToString() + "," + cmdTime.ToString();
                }

            }

            para = fCmdRet.ToString();
            //Console.WriteLine("~~~~~~~~~~~~~"+para); 

        }


    }
}
