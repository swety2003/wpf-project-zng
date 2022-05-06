using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WPF_Project.Utils.DoorSdk;

namespace WPF_Project.Utils
{
    //门禁接口
    internal class DoorAcc
    {
        private string ip { get; set; }
        public string port { get; set; }
        private string username { get; set; }
        private string password { get; set; }

        public static int m_UserID = -1;
        // 卡
        public Int32 m_lGetCardCfgHandle = -1;
        public Int32 m_lSetCardCfgHandle = -1;
        public Int32 m_lDelCardCfgHandle = -1;
        public IntPtr hwnd;

        // 人脸
        private int m_lGetFaceCfgHandle = -1;
        private int m_lSetFaceCfgHandle = -1;
        private int m_lCapFaceCfgHandle = -1;
        private string facePath = null;

        // 布防
        private CHCNetSDK.MSGCallBack m_falarmData = null;
        private string path = null;
        private int m_lLogNum = 0;
        private string ShowData = null;


        public DoorAcc(string ip, string port, string username, string password) 
        {
            this.ip = ip;
            this.port = port;
            this.username = username;
            this.password = password;
        }

        //初始化
        public void Init()
        {
            try
            {
                if (CHCNetSDK.NET_DVR_Init() == false)
                {
                    MessageBox.Show("门禁初始化失败!");
                    return;
                }
                CHCNetSDK.NET_DVR_SetLogToFile(3, "./", false);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //登录
        public void login()
        {
            if (m_UserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout_V30(m_UserID);
                m_UserID = -1;
            }
            CHCNetSDK.NET_DVR_USER_LOGIN_INFO struLoginInfo = new CHCNetSDK.NET_DVR_USER_LOGIN_INFO();
            CHCNetSDK.NET_DVR_DEVICEINFO_V40 struDeviceInfoV40 = new CHCNetSDK.NET_DVR_DEVICEINFO_V40();
            struDeviceInfoV40.struDeviceV30.sSerialNumber = new byte[CHCNetSDK.SERIALNO_LEN];

            struLoginInfo.sDeviceAddress = this.ip;
            struLoginInfo.sUserName = this.username;
            struLoginInfo.sPassword = this.password;
            ushort.TryParse(this.port, out struLoginInfo.wPort);

            int lUserID = -1;
            lUserID = CHCNetSDK.NET_DVR_Login_V40(ref struLoginInfo, ref struDeviceInfoV40);
            if (lUserID >= 0)
            {
                m_UserID = lUserID;
                //MessageBox.Show("登录成功！");

            }
            else
            {
                uint nErr = CHCNetSDK.NET_DVR_GetLastError();
                if (nErr == CHCNetSDK.NET_DVR_PASSWORD_ERROR)
                {
                    MessageBox.Show("用户名或密码错误!");
                    if (1 == struDeviceInfoV40.bySupportLock)
                    {
                        string strTemp1 = string.Format("还有 {0} 尝试机会", struDeviceInfoV40.byRetryLoginTime);
                        MessageBox.Show(strTemp1);
                    }
                }
                else if (nErr == CHCNetSDK.NET_DVR_USER_LOCKED)
                {
                    if (1 == struDeviceInfoV40.bySupportLock)
                    {
                        string strTemp1 = string.Format("用户已锁定，剩余锁定时间为 {0}", struDeviceInfoV40.dwSurplusLockTime);
                        MessageBox.Show(strTemp1);
                    }
                }
                else
                {
                    MessageBox.Show("网络错误或设备正忙!");
                }
            }
        }

        //布防
        public void doorDeploy()
        {
            if (m_UserID < 0)
            {
                MessageBox.Show("请先登录!");
                return;
            }
            else
            {
                CHCNetSDK.NET_DVR_SETUPALARM_PARAM struSetupAlarmParam = new CHCNetSDK.NET_DVR_SETUPALARM_PARAM();
                struSetupAlarmParam.dwSize = (uint)Marshal.SizeOf(struSetupAlarmParam);
                struSetupAlarmParam.byLevel = 1;
                struSetupAlarmParam.byAlarmInfoType = 1;
                struSetupAlarmParam.byDeployType = (byte)1;

                if (CHCNetSDK.NET_DVR_SetupAlarmChan_V41(m_UserID, ref struSetupAlarmParam) < 0)
                {
                    MessageBox.Show("设置警报失败 error: " + CHCNetSDK.NET_DVR_GetLastError(), "设置报警失败");
                    return;
                }
                else
                {
                    //MessageBox.Show("设置警报成功");
                }

                m_falarmData = new CHCNetSDK.MSGCallBack(MsgCallback);
                if (CHCNetSDK.NET_DVR_SetDVRMessageCallBack_V50(0, m_falarmData, IntPtr.Zero))
                {
                    //MessageBox.Show("设置设备消息回调成功");
                }
                else
                {
                    MessageBox.Show("设置设备消息回调失败");
                }
            }
        }

        //消息回调
        private void MsgCallback(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            switch (lCommand)
            {
                case CHCNetSDK.COMM_ALARM_ACS:
                    ProcessCommAlarmACS(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                default:
                    break;
            }
        }

        private void ProcessCommAlarmACS(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            CHCNetSDK.NET_DVR_ACS_ALARM_INFO struAcsAlarmInfo = new CHCNetSDK.NET_DVR_ACS_ALARM_INFO();
            struAcsAlarmInfo = (CHCNetSDK.NET_DVR_ACS_ALARM_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_DVR_ACS_ALARM_INFO));
            CHCNetSDK.NET_DVR_LOG_V30 struFileInfo = new CHCNetSDK.NET_DVR_LOG_V30();
            struFileInfo.dwMajorType = struAcsAlarmInfo.dwMajor;
            struFileInfo.dwMinorType = struAcsAlarmInfo.dwMinor;
            char[] csTmp = new char[256];

            if (CHCNetSDK.MAJOR_ALARM == struFileInfo.dwMajorType)
            {
                TypeMap.AlarmMinorTypeMap(struFileInfo, csTmp);
            }
            else if (CHCNetSDK.MAJOR_OPERATION == struFileInfo.dwMajorType)
            {
                TypeMap.OperationMinorTypeMap(struFileInfo, csTmp);
            }
            else if (CHCNetSDK.MAJOR_EXCEPTION == struFileInfo.dwMajorType)
            {
                TypeMap.ExceptionMinorTypeMap(struFileInfo, csTmp);
            }
            else if (CHCNetSDK.MAJOR_EVENT == struFileInfo.dwMajorType)
            {
                TypeMap.EventMinorTypeMap(struFileInfo, csTmp);
            }

            String szInfo = new String(csTmp).TrimEnd('\0');
            String szInfoBuf = null;
            szInfoBuf = szInfo;
            /**************************************************/
            String name = System.Text.Encoding.UTF8.GetString(struAcsAlarmInfo.sNetUser).TrimEnd('\0');
            for (int i = 0; i < struAcsAlarmInfo.sNetUser.Length; i++)
            {
                if (struAcsAlarmInfo.sNetUser[i] == 0)
                {
                    name = name.Substring(0, i);
                    break;
                }
            }
            /**************************************************/

            szInfoBuf = string.Format("{0} time:{1,4}-{2:D2}-{3} {4:D2}:{5:D2}:{6:D2}, [{7}]({8})", szInfo, struAcsAlarmInfo.struTime.dwYear, struAcsAlarmInfo.struTime.dwMonth,
                struAcsAlarmInfo.struTime.dwDay, struAcsAlarmInfo.struTime.dwHour, struAcsAlarmInfo.struTime.dwMinute, struAcsAlarmInfo.struTime.dwSecond,
                struAcsAlarmInfo.struRemoteHostAddr.sIpV4, name);

            if (struAcsAlarmInfo.struAcsEventInfo.byCardNo[0] != 0)
            {
                szInfoBuf = szInfoBuf + "+Card Number:" + System.Text.Encoding.UTF8.GetString(struAcsAlarmInfo.struAcsEventInfo.byCardNo).TrimEnd('\0');
            }
            String[] szCardType = { "normal card", "disabled card", "blacklist card", "night watch card", "stress card", "super card", "guest card" };
            byte byCardType = struAcsAlarmInfo.struAcsEventInfo.byCardType;

            if (byCardType != 0 && byCardType <= szCardType.Length)
            {
                szInfoBuf = szInfoBuf + "+Card Type:" + szCardType[byCardType - 1];
            }

            if (struAcsAlarmInfo.struAcsEventInfo.dwCardReaderNo != 0)
            {
                szInfoBuf = szInfoBuf + "+Card Reader Number:" + struAcsAlarmInfo.struAcsEventInfo.dwCardReaderNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwDoorNo != 0)
            {
                szInfoBuf = szInfoBuf + "+Door Number:" + struAcsAlarmInfo.struAcsEventInfo.dwDoorNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwVerifyNo != 0)
            {
                szInfoBuf = szInfoBuf + "+Multiple Card Authentication Serial Number:" + struAcsAlarmInfo.struAcsEventInfo.dwVerifyNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwAlarmInNo != 0)
            {
                szInfoBuf = szInfoBuf + "+Alarm Input Number:" + struAcsAlarmInfo.struAcsEventInfo.dwAlarmInNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwAlarmOutNo != 0)
            {
                szInfoBuf = szInfoBuf + "+Alarm Output Number:" + struAcsAlarmInfo.struAcsEventInfo.dwAlarmOutNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwCaseSensorNo != 0)
            {
                szInfoBuf = szInfoBuf + "+Event Trigger Number:" + struAcsAlarmInfo.struAcsEventInfo.dwCaseSensorNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwRs485No != 0)
            {
                szInfoBuf = szInfoBuf + "+RS485 Channel Number:" + struAcsAlarmInfo.struAcsEventInfo.dwRs485No;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwMultiCardGroupNo != 0)
            {
                szInfoBuf = szInfoBuf + "+Multi Recombinant Authentication ID:" + struAcsAlarmInfo.struAcsEventInfo.dwMultiCardGroupNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.byCardReaderKind != 0)
            {
                szInfoBuf = szInfoBuf + "+CardReaderKind:" + struAcsAlarmInfo.struAcsEventInfo.byCardReaderKind.ToString();
            }
            if (struAcsAlarmInfo.struAcsEventInfo.wAccessChannel >= 0)
            {
                szInfoBuf = szInfoBuf + "+wAccessChannel:" + struAcsAlarmInfo.struAcsEventInfo.wAccessChannel;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.dwEmployeeNo != 0)
            {
                szInfoBuf = szInfoBuf + "+EmployeeNo:" + struAcsAlarmInfo.struAcsEventInfo.dwEmployeeNo;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.byDeviceNo != 0)
            {
                szInfoBuf = szInfoBuf + "+byDeviceNo:" + struAcsAlarmInfo.struAcsEventInfo.byDeviceNo.ToString();
            }
            if (struAcsAlarmInfo.struAcsEventInfo.wLocalControllerID >= 0)
            {
                szInfoBuf = szInfoBuf + "+wLocalControllerID:" + struAcsAlarmInfo.struAcsEventInfo.wLocalControllerID;
            }
            if (struAcsAlarmInfo.struAcsEventInfo.byInternetAccess >= 0)
            {
                szInfoBuf = szInfoBuf + "+byInternetAccess:" + struAcsAlarmInfo.struAcsEventInfo.byInternetAccess.ToString();
            }
            if (struAcsAlarmInfo.struAcsEventInfo.byType >= 0)
            {
                szInfoBuf = szInfoBuf + "+byType:" + struAcsAlarmInfo.struAcsEventInfo.byType.ToString();
            }
            if (struAcsAlarmInfo.struAcsEventInfo.bySwipeCardType != 0)
            {
                szInfoBuf = szInfoBuf + "+bySwipeCardType:" + struAcsAlarmInfo.struAcsEventInfo.bySwipeCardType.ToString();
            }
            //其它消息先不罗列了......

            if (struAcsAlarmInfo.dwPicDataLen > 0)
            {
                path = null;
                Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
                path = string.Format(@"" + System.Environment.CurrentDirectory + "\\" + "Picture/ACS_LocalTime{0}_{1}.bmp", szInfo, rand.Next());
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    int iLen = (int)struAcsAlarmInfo.dwPicDataLen;
                    byte[] by = new byte[iLen];
                    Marshal.Copy(struAcsAlarmInfo.pPicData, by, 0, iLen);
                    fs.Write(by, 0, iLen);
                    fs.Close();
                }
                szInfoBuf = szInfoBuf + "SavePath:" + path;
            }

            if (szInfoBuf.Contains("EmployeeNo:"))
            {
                string employeeNo = MidStrEx(szInfoBuf, "EmployeeNo:", "+");
                MessageBox.Show("工号：" + employeeNo);
            }
            //Dispatcher.BeginInvoke(new Action(
            //    delegate {
            //        var p = new AlarmInfo((++m_lLogNum).ToString(), DateTime.Now.ToString(), szInfoBuf);
            //        doorListenListView.Items.Add(p);
            //    }
            //));

        }

        private string MidStrEx(string sourse, string startstr, string endstr)
        {
            string result = string.Empty;
            int startindex, endindex;
            try
            {
                startindex = sourse.IndexOf(startstr);
                if (startindex == -1)
                    return result;
                string tmpstr = sourse.Substring(startindex + startstr.Length);
                endindex = tmpstr.IndexOf(endstr);
                if (endindex == -1)
                    return result;
                result = tmpstr.Remove(endindex);
            }
            catch (Exception e)
            {
                MessageBox.Show("MidStrEx Err:" + e.Message);
            }
            return result;
        }

        // 根据卡号获取用户信息(没做返回)
        public void getUserByCardNo(string cardNo)
        {
            string doorCardNo;
            string doorEmployeeNo;
            string doorUserName;

            if (m_lGetCardCfgHandle != -1)
            {
                if (CHCNetSDK.NET_DVR_StopRemoteConfig(m_lGetCardCfgHandle))
                {
                    m_lGetCardCfgHandle = -1;
                }
            }
            CHCNetSDK.NET_DVR_CARD_COND struCond = new CHCNetSDK.NET_DVR_CARD_COND();
            struCond.Init();
            struCond.dwSize = (uint)Marshal.SizeOf(struCond);
            struCond.dwCardNum = 1;
            IntPtr ptrStruCond = Marshal.AllocHGlobal((int)struCond.dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);

            CHCNetSDK.NET_DVR_CARD_RECORD struData = new CHCNetSDK.NET_DVR_CARD_RECORD();
            struData.Init();
            struData.dwSize = (uint)Marshal.SizeOf(struData);
            byte[] byTempCardNo = new byte[CHCNetSDK.ACS_CARD_NO_LEN];
            byTempCardNo = System.Text.Encoding.UTF8.GetBytes(cardNo);
            for (int i = 0; i < byTempCardNo.Length; i++)
            {
                struData.byCardNo[i] = byTempCardNo[i];
            }
            IntPtr ptrStruData = Marshal.AllocHGlobal((int)struData.dwSize);
            Marshal.StructureToPtr(struData, ptrStruData, false);

            CHCNetSDK.NET_DVR_CARD_SEND_DATA struSendData = new CHCNetSDK.NET_DVR_CARD_SEND_DATA();
            struSendData.Init();
            struSendData.dwSize = (uint)Marshal.SizeOf(struSendData);
            for (int i = 0; i < byTempCardNo.Length; i++)
            {
                struSendData.byCardNo[i] = byTempCardNo[i];
            }
            IntPtr ptrStruSendData = Marshal.AllocHGlobal((int)struSendData.dwSize);
            Marshal.StructureToPtr(struSendData, ptrStruSendData, false);

            m_lGetCardCfgHandle = CHCNetSDK.NET_DVR_StartRemoteConfig(m_UserID, CHCNetSDK.NET_DVR_GET_CARD, ptrStruCond, (int)struCond.dwSize, null, hwnd);
            if (m_lGetCardCfgHandle < 0)
            {
                MessageBox.Show("NET_DVR_GET_CARD error: " + CHCNetSDK.NET_DVR_GetLastError());
                Marshal.FreeHGlobal(ptrStruCond);
                return;
            }
            else
            {
                int dwState = (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS;
                uint dwReturned = 0;
                while (true)
                {
                    dwState = CHCNetSDK.NET_DVR_SendWithRecvRemoteConfig(m_lGetCardCfgHandle, ptrStruSendData, struSendData.dwSize, ptrStruData, struData.dwSize, ref dwReturned);
                    struData = (CHCNetSDK.NET_DVR_CARD_RECORD)Marshal.PtrToStructure(ptrStruData, typeof(CHCNetSDK.NET_DVR_CARD_RECORD));
                    if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_NEEDWAIT)
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    else if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FAILED)
                    {
                        MessageBox.Show("NET_DVR_GET_CARD fail error: " + CHCNetSDK.NET_DVR_GetLastError());
                    }
                    else if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS)
                    {
                        doorCardNo = System.Text.Encoding.Default.GetString(struData.byCardNo);
                        //textBoxCardRightPlan.Text = struData.wCardRightPlan[0].ToString();
                        doorEmployeeNo = struData.dwEmployeeNo.ToString();
                        doorUserName = System.Text.Encoding.Default.GetString(struData.byName);
                        MessageBox.Show("NET_DVR_GET_CARD success");
                    }
                    else if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FINISH)
                    {
                        MessageBox.Show("NET_DVR_GET_CARD finish");
                        break;
                    }
                    else if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_EXCEPTION)
                    {
                        MessageBox.Show("NET_DVR_GET_CARD exception error: " + CHCNetSDK.NET_DVR_GetLastError());
                        break;
                    }
                    else
                    {
                        MessageBox.Show("unknown status error: " + CHCNetSDK.NET_DVR_GetLastError());
                        break;
                    }
                }
            }
            CHCNetSDK.NET_DVR_StopRemoteConfig(m_lGetCardCfgHandle);
            m_lGetCardCfgHandle = -1;
            Marshal.FreeHGlobal(ptrStruSendData);
            Marshal.FreeHGlobal(ptrStruData);



        }

        // 卡下发到设备
        // 工号employeeNo 姓名userName 卡号cardNo
        public void uploadCardNo(string employeeNo,string userName,string cardNo)
        {
            if (m_lSetCardCfgHandle != -1)
            {
                if (CHCNetSDK.NET_DVR_StopRemoteConfig(m_lSetCardCfgHandle))
                {
                    m_lSetCardCfgHandle = -1;
                }
            }

            CHCNetSDK.NET_DVR_CARD_COND struCond = new CHCNetSDK.NET_DVR_CARD_COND();
            struCond.Init();
            struCond.dwSize = (uint)Marshal.SizeOf(struCond);
            struCond.dwCardNum = 1;
            IntPtr ptrStruCond = Marshal.AllocHGlobal((int)struCond.dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);

            m_lSetCardCfgHandle = CHCNetSDK.NET_DVR_StartRemoteConfig(m_UserID, CHCNetSDK.NET_DVR_SET_CARD, ptrStruCond, (int)struCond.dwSize, null, IntPtr.Zero);
            if (m_lSetCardCfgHandle < 0)
            {
                MessageBox.Show("NET_DVR_SET_CARD error:" + CHCNetSDK.NET_DVR_GetLastError());
                Marshal.FreeHGlobal(ptrStruCond);
                return;
            }
            else
            {
                SendCardData(employeeNo,userName,cardNo);
                Marshal.FreeHGlobal(ptrStruCond);
            }
        }

        private void SendCardData(string employeeNo,string userName,string cardNo)
        {
            CHCNetSDK.NET_DVR_CARD_RECORD struData = new CHCNetSDK.NET_DVR_CARD_RECORD();
            struData.Init();
            struData.dwSize = (uint)Marshal.SizeOf(struData);
            struData.byCardType = 1;
            byte[] byTempCardNo = new byte[CHCNetSDK.ACS_CARD_NO_LEN];
            byTempCardNo = System.Text.Encoding.UTF8.GetBytes(cardNo);
            for (int i = 0; i < byTempCardNo.Length; i++)
            {
                struData.byCardNo[i] = byTempCardNo[i];
            }
            ushort.TryParse("1", out struData.wCardRightPlan[0]);
            uint.TryParse(employeeNo, out struData.dwEmployeeNo);
            byte[] byTempName = new byte[CHCNetSDK.NAME_LEN];
            byTempName = System.Text.Encoding.Default.GetBytes(userName);
            for (int i = 0; i < byTempName.Length; i++)
            {
                struData.byName[i] = byTempName[i];
            }
            struData.struValid.byEnable = 1;
            struData.struValid.struBeginTime.wYear = 2000;
            struData.struValid.struBeginTime.byMonth = 1;
            struData.struValid.struBeginTime.byDay = 1;
            struData.struValid.struBeginTime.byHour = 11;
            struData.struValid.struBeginTime.byMinute = 11;
            struData.struValid.struBeginTime.bySecond = 11;
            struData.struValid.struEndTime.wYear = 2030;
            struData.struValid.struEndTime.byMonth = 1;
            struData.struValid.struEndTime.byDay = 1;
            struData.struValid.struEndTime.byHour = 11;
            struData.struValid.struEndTime.byMinute = 11;
            struData.struValid.struEndTime.bySecond = 11;
            struData.byDoorRight[0] = 1;
            struData.wCardRightPlan[0] = 1;
            IntPtr ptrStruData = Marshal.AllocHGlobal((int)struData.dwSize);
            Marshal.StructureToPtr(struData, ptrStruData, false);

            CHCNetSDK.NET_DVR_CARD_STATUS struStatus = new CHCNetSDK.NET_DVR_CARD_STATUS();
            struStatus.Init();
            struStatus.dwSize = (uint)Marshal.SizeOf(struStatus);
            IntPtr ptrdwState = Marshal.AllocHGlobal((int)struStatus.dwSize);
            Marshal.StructureToPtr(struStatus, ptrdwState, false);

            int dwState = (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS;
            uint dwReturned = 0;
            while (true)
            {
                dwState = CHCNetSDK.NET_DVR_SendWithRecvRemoteConfig(m_lSetCardCfgHandle, ptrStruData, struData.dwSize, ptrdwState, struStatus.dwSize, ref dwReturned);
                struStatus = (CHCNetSDK.NET_DVR_CARD_STATUS)Marshal.PtrToStructure(ptrdwState, typeof(CHCNetSDK.NET_DVR_CARD_STATUS));
                if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_NEEDWAIT)
                {
                    Thread.Sleep(10);
                    continue;
                }
                else if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FAILED)
                {
                    MessageBox.Show("NET_DVR_SET_CARD fail error: " + CHCNetSDK.NET_DVR_GetLastError());
                }
                else if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS)
                {
                    if (struStatus.dwErrorCode != 0)
                    {
                        MessageBox.Show("NET_DVR_SET_CARD success but errorCode:" + struStatus.dwErrorCode);
                    }
                    else
                    {
                        MessageBox.Show("NET_DVR_SET_CARD success");
                    }
                }
                else if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FINISH)
                {
                    MessageBox.Show("NET_DVR_SET_CARD finish");
                    break;
                }
                else if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_EXCEPTION)
                {
                    MessageBox.Show("NET_DVR_SET_CARD exception error: " + CHCNetSDK.NET_DVR_GetLastError());
                    break;
                }
                else
                {
                    //MessageBox.Show("unknown status error: " + CHCNetSDK.NET_DVR_GetLastError());
                    break;
                }
            }
            CHCNetSDK.NET_DVR_StopRemoteConfig(m_lSetCardCfgHandle);
            m_lSetCardCfgHandle = -1;
            Marshal.FreeHGlobal(ptrStruData);
            Marshal.FreeHGlobal(ptrdwState);
            return;
        }

        // 根据卡号删除卡
        public void deleteCardNo(string cardNo)
        {
            if (m_lDelCardCfgHandle != -1)
            {
                if (CHCNetSDK.NET_DVR_StopRemoteConfig(m_lDelCardCfgHandle))
                {
                    m_lDelCardCfgHandle = -1;
                }
            }
            CHCNetSDK.NET_DVR_CARD_COND struCond = new CHCNetSDK.NET_DVR_CARD_COND();
            struCond.Init();
            struCond.dwSize = (uint)Marshal.SizeOf(struCond);
            struCond.dwCardNum = 1;
            IntPtr ptrStruCond = Marshal.AllocHGlobal((int)struCond.dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);

            CHCNetSDK.NET_DVR_CARD_SEND_DATA struSendData = new CHCNetSDK.NET_DVR_CARD_SEND_DATA();
            struSendData.Init();
            struSendData.dwSize = (uint)Marshal.SizeOf(struSendData);
            byte[] byTempCardNo = new byte[CHCNetSDK.ACS_CARD_NO_LEN];
            byTempCardNo = System.Text.Encoding.UTF8.GetBytes(cardNo);
            for (int i = 0; i < byTempCardNo.Length; i++)
            {
                struSendData.byCardNo[i] = byTempCardNo[i];
            }
            IntPtr ptrStruSendData = Marshal.AllocHGlobal((int)struSendData.dwSize);
            Marshal.StructureToPtr(struSendData, ptrStruSendData, false);

            CHCNetSDK.NET_DVR_CARD_STATUS struStatus = new CHCNetSDK.NET_DVR_CARD_STATUS();
            struStatus.Init();
            struStatus.dwSize = (uint)Marshal.SizeOf(struStatus);
            IntPtr ptrdwState = Marshal.AllocHGlobal((int)struStatus.dwSize);
            Marshal.StructureToPtr(struStatus, ptrdwState, false);

            m_lGetCardCfgHandle = CHCNetSDK.NET_DVR_StartRemoteConfig(m_UserID, CHCNetSDK.NET_DVR_DEL_CARD, ptrStruCond, (int)struCond.dwSize, null, hwnd);
            if (m_lGetCardCfgHandle < 0)
            {
                MessageBox.Show("NET_DVR_DEL_CARD error:" + CHCNetSDK.NET_DVR_GetLastError());
                Marshal.FreeHGlobal(ptrStruCond);
                return;
            }
            else
            {
                int dwState = (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS;
                uint dwReturned = 0;
                while (true)
                {
                    dwState = CHCNetSDK.NET_DVR_SendWithRecvRemoteConfig(m_lGetCardCfgHandle, ptrStruSendData, struSendData.dwSize, ptrdwState, struStatus.dwSize, ref dwReturned);
                    if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_NEEDWAIT)
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    else if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FAILED)
                    {
                        MessageBox.Show("NET_DVR_DEL_CARD fail error: " + CHCNetSDK.NET_DVR_GetLastError());
                    }
                    else if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_SUCCESS)
                    {
                        MessageBox.Show("NET_DVR_DEL_CARD success");
                    }
                    else if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_FINISH)
                    {
                        MessageBox.Show("NET_DVR_DEL_CARD finish");
                        break;
                    }
                    else if (dwState == (int)CHCNetSDK.NET_SDK_SENDWITHRECV_STATUS.NET_SDK_CONFIG_STATUS_EXCEPTION)
                    {
                        MessageBox.Show("NET_DVR_DEL_CARD exception error: " + CHCNetSDK.NET_DVR_GetLastError());
                        break;
                    }
                    else
                    {
                        MessageBox.Show("unknown status error: " + CHCNetSDK.NET_DVR_GetLastError());
                        break;
                    }
                }
            }
            CHCNetSDK.NET_DVR_StopRemoteConfig(m_lDelCardCfgHandle);
            m_lDelCardCfgHandle = -1;
            Marshal.FreeHGlobal(ptrStruSendData);
            Marshal.FreeHGlobal(ptrdwState);
        }

        // 根据卡号获取人脸图片
        public void getFaceByCardNo(string cardNo)
        {
            if (m_lGetFaceCfgHandle != -1)
            {
                CHCNetSDK.NET_DVR_StopRemoteConfig(m_lGetFaceCfgHandle);
                m_lGetFaceCfgHandle = -1;
            }


            CHCNetSDK.NET_DVR_FACE_COND struCond = new CHCNetSDK.NET_DVR_FACE_COND();
            struCond.init();
            struCond.dwSize = Marshal.SizeOf(struCond);
            int dwSize = struCond.dwSize;
            // 读卡器
            /**
            if (doorCardReaderNo.Text.ToString() == "")
            {
                struCond.dwEnableReaderNo = 0;
            }
            else
            {
                int.TryParse(doorCardReaderNo.Text.ToString(), out struCond.dwEnableReaderNo);
            }**/
            struCond.dwFaceNum = 1;//人脸数量是1
            byte[] byTemp = System.Text.Encoding.UTF8.GetBytes(cardNo);
            for (int i = 0; i < byTemp.Length; i++)
            {
                struCond.byCardNo[i] = byTemp[i];
            }

            IntPtr ptrStruCond = Marshal.AllocHGlobal(dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);

            m_lGetFaceCfgHandle = CHCNetSDK.NET_DVR_StartRemoteConfig(m_UserID, CHCNetSDK.NET_DVR_GET_FACE, ptrStruCond, dwSize, null, IntPtr.Zero);
            if (m_lGetFaceCfgHandle == -1)
            {
                Marshal.FreeHGlobal(ptrStruCond);
                MessageBox.Show("NET_DVR_GET_FACE_FAIL, ERROR CODE" + CHCNetSDK.NET_DVR_GetLastError().ToString());
                return;
            }

            bool Flag = true;
            int dwStatus = 0;

            CHCNetSDK.NET_DVR_FACE_RECORD struRecord = new CHCNetSDK.NET_DVR_FACE_RECORD();
            struRecord.init();
            struRecord.dwSize = Marshal.SizeOf(struRecord);
            int dwOutBuffSize = struRecord.dwSize;
            while (Flag)
            {
                dwStatus = CHCNetSDK.NET_DVR_GetNextRemoteConfig(m_lGetFaceCfgHandle, ref struRecord, dwOutBuffSize);
                switch (dwStatus)
                {
                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_SUCCESS://成功读取到数据，处理完本次数据后需调用next
                        ProcessFaceData(ref struRecord, ref Flag);
                        break;
                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        break;
                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_FAILED:
                        CHCNetSDK.NET_DVR_StopRemoteConfig(m_lGetFaceCfgHandle);
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_FAILED" + CHCNetSDK.NET_DVR_GetLastError().ToString());
                        Flag = false;
                        break;
                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_FINISH:
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_FINISH");
                        CHCNetSDK.NET_DVR_StopRemoteConfig(m_lGetFaceCfgHandle);
                        Flag = false;
                        break;
                    default:
                        MessageBox.Show("NET_SDK_GET_STATUS_UNKOWN" + CHCNetSDK.NET_DVR_GetLastError().ToString());
                        Flag = false;
                        CHCNetSDK.NET_DVR_StopRemoteConfig(m_lGetFaceCfgHandle);
                        break;
                }
            }

            Marshal.FreeHGlobal(ptrStruCond);
        }

        private void ProcessFaceData(ref CHCNetSDK.NET_DVR_FACE_RECORD struRecord, ref bool Flag)
        {
            string strpath = null;
            DateTime dt = DateTime.Now;
            strpath = string.Format("FacePicture.jpg");

            if (0 == struRecord.dwFaceLen)
            {
                return;
            }
            try
            {
                using (FileStream fs = new FileStream(strpath, FileMode.OpenOrCreate))
                {
                    int FaceLen = struRecord.dwFaceLen;
                    byte[] by = new byte[FaceLen];
                    Marshal.Copy(struRecord.pFaceBuffer, by, 0, FaceLen);
                    fs.Write(by, 0, FaceLen);
                    fs.Close();
                }
                //载入图片
                // Read byte[] from png file
                BinaryReader binReader = new BinaryReader(File.Open(System.Environment.CurrentDirectory + "\\" + strpath, FileMode.Open));
                FileInfo fileInfo = new FileInfo(System.Environment.CurrentDirectory + "\\" + strpath);
                byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
                binReader.Close();

                // Init bitmap
                //BitmapImage bitmap = new BitmapImage();
                //bitmap.BeginInit();
                //bitmap.StreamSource = new MemoryStream(bytes);
                //bitmap.EndInit(); 
                //imageFace.Source = bitmap;

                facePath = string.Format("{0}\\{1}", Environment.CurrentDirectory, strpath);
                //pictureBoxFace.Image = Image.FromFile(strpath);
                //textBoxFilePath.Text = string.Format("{0}\\{1}", Environment.CurrentDirectory, strpath);
            }
            catch
            {
                Flag = false;
                CHCNetSDK.NET_DVR_StopRemoteConfig(m_lGetFaceCfgHandle);
                MessageBox.Show("ProcessFingerData failed");
            }
        }


        // 人脸采集
        public void captureFace()
        {
            if (m_lCapFaceCfgHandle != -1)
            {
                CHCNetSDK.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                m_lCapFaceCfgHandle = -1;
            }
            //imageFace.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/head.jpg"));

            CHCNetSDK.NET_DVR_CAPTURE_FACE_COND struCond = new CHCNetSDK.NET_DVR_CAPTURE_FACE_COND();
            struCond.init();
            struCond.dwSize = Marshal.SizeOf(struCond);
            int dwInBufferSize = struCond.dwSize;
            IntPtr ptrStruCond = Marshal.AllocHGlobal(dwInBufferSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);
            m_lCapFaceCfgHandle = CHCNetSDK.NET_DVR_StartRemoteConfig(m_UserID, CHCNetSDK.NET_DVR_CAPTURE_FACE_INFO, ptrStruCond, dwInBufferSize, null, IntPtr.Zero);
            if (-1 == m_lCapFaceCfgHandle)
            {
                Marshal.FreeHGlobal(ptrStruCond);
                MessageBox.Show("NET_DVR_CAP_FACE_FAIL, ERROR CODE" + CHCNetSDK.NET_DVR_GetLastError().ToString());
                return;
            }

            CHCNetSDK.NET_DVR_CAPTURE_FACE_CFG struFaceCfg = new CHCNetSDK.NET_DVR_CAPTURE_FACE_CFG();
            struFaceCfg.init();
            int dwStatus = 0;
            int dwOutBuffSize = Marshal.SizeOf(struFaceCfg);
            bool Flag = true;
            while (Flag)
            {
                dwStatus = CHCNetSDK.NET_DVR_GetNextRemoteConfig(m_lCapFaceCfgHandle, ref struFaceCfg, dwOutBuffSize);
                switch (dwStatus)
                {
                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_SUCCESS://成功读取到数据，处理完本次数据后需调用next
                        ProcessCapFaceData(ref struFaceCfg, ref Flag);
                        break;
                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        break;
                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_FAILED:
                        CHCNetSDK.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_FAILED" + CHCNetSDK.NET_DVR_GetLastError().ToString());
                        Flag = false;
                        break;
                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_FINISH:
                        CHCNetSDK.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                        Flag = false;
                        break;
                    default:
                        MessageBox.Show("NET_SDK_GET_STATUS_UNKOWN" + CHCNetSDK.NET_DVR_GetLastError().ToString());
                        Flag = false;
                        CHCNetSDK.NET_DVR_StopRemoteConfig(m_lCapFaceCfgHandle);
                        break;
                }
            }
            Marshal.FreeHGlobal(ptrStruCond);
        }

        private void ProcessCapFaceData(ref CHCNetSDK.NET_DVR_CAPTURE_FACE_CFG struFaceCfg, ref bool flag)
        {
            if (0 == struFaceCfg.dwFacePicSize)
            {
                return;
            }
            string strpath = null;
            DateTime dt = DateTime.Now;
            strpath = string.Format("capture.jpg", Environment.CurrentDirectory);
            try
            {
                using (FileStream fs = new FileStream(strpath, FileMode.OpenOrCreate))
                {
                    int FaceLen = struFaceCfg.dwFacePicSize;
                    byte[] by = new byte[FaceLen];
                    Marshal.Copy(struFaceCfg.pFacePicBuffer, by, 0, FaceLen);
                    fs.Write(by, 0, FaceLen);
                    fs.Close();
                }
                //载入图片 
                // Read byte[] from png file
                BinaryReader binReader = new BinaryReader(File.Open(System.Environment.CurrentDirectory + "\\" + strpath, FileMode.Open));
                FileInfo fileInfo = new FileInfo(System.Environment.CurrentDirectory + "\\" + strpath);
                byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
                binReader.Close();

                // Init bitmap 采集成功显示
                //BitmapImage bitmap = new BitmapImage();
                //bitmap.BeginInit();
                //bitmap.StreamSource = new MemoryStream(bytes);
                //bitmap.EndInit(); 
                //imageFace.Source = bitmap;

                facePath = string.Format("{0}\\{1}", Environment.CurrentDirectory, strpath);
                MessageBox.Show("Capture succeed");
            }
            catch
            {
                flag = false;
                MessageBox.Show("capature data wrong");
            }
        }

        //人脸下发到设备 //人脸读卡器状态，按字节表示，0-失败，1-成功，2-重试或人脸质量差，3-内存已满(人脸数据满)，4-已存在该人脸，5-非法人脸ID，6-算法建模失败，7-未下发卡权限，8-未定义（保留），9-人眼间距小距小，10-图片数据长度小于1KB，11-图片格式不符（png/jpg/bmp），12-图片像素数量超过上限，13-图片像素数量低于下限，14-图片信息校验失败，15-图片解码失败，16-人脸检测失败，17-人脸评分失败
        public void uploadFace(string cardNo)
        {
            CHCNetSDK.NET_DVR_FACE_COND struCond = new CHCNetSDK.NET_DVR_FACE_COND();
            struCond.init();
            struCond.dwSize = Marshal.SizeOf(struCond);
            struCond.dwFaceNum = 1;
            int.TryParse("1", out struCond.dwEnableReaderNo);
            byte[] byTemp = System.Text.Encoding.UTF8.GetBytes(cardNo);
            for (int i = 0; i < byTemp.Length; i++)
            {
                struCond.byCardNo[i] = byTemp[i];
            }

            int dwInBufferSize = struCond.dwSize;
            IntPtr ptrstruCond = Marshal.AllocHGlobal(dwInBufferSize);
            Marshal.StructureToPtr(struCond, ptrstruCond, false);
            m_lSetFaceCfgHandle = CHCNetSDK.NET_DVR_StartRemoteConfig(m_UserID, CHCNetSDK.NET_DVR_SET_FACE, ptrstruCond, dwInBufferSize, null, IntPtr.Zero);
            if (-1 == m_lSetFaceCfgHandle)
            {
                Marshal.FreeHGlobal(ptrstruCond);
                MessageBox.Show("NET_DVR_SET_FACE_FAIL, ERROR CODE" + CHCNetSDK.NET_DVR_GetLastError().ToString());
                return;
            }

            CHCNetSDK.NET_DVR_FACE_RECORD struRecord = new CHCNetSDK.NET_DVR_FACE_RECORD();
            struRecord.init();
            struRecord.dwSize = Marshal.SizeOf(struRecord);

            byte[] byRecordNo = System.Text.Encoding.UTF8.GetBytes(cardNo);
            for (int i = 0; i < byRecordNo.Length; i++)
            {
                struRecord.byCardNo[i] = byRecordNo[i];
            }

            ReadFaceData(ref struRecord);
            int dwInBuffSize = Marshal.SizeOf(struRecord);
            int dwStatus = 0;

            CHCNetSDK.NET_DVR_FACE_STATUS struStatus = new CHCNetSDK.NET_DVR_FACE_STATUS();
            struStatus.init();
            struStatus.dwSize = Marshal.SizeOf(struStatus);
            int dwOutBuffSize = struStatus.dwSize;
            IntPtr ptrOutDataLen = Marshal.AllocHGlobal(sizeof(int));
            bool Flag = true;
            while (Flag)
            {
                dwStatus = CHCNetSDK.NET_DVR_SendWithRecvRemoteConfig(m_lSetFaceCfgHandle, ref struRecord, dwInBuffSize, ref struStatus, dwOutBuffSize, ptrOutDataLen);
                switch (dwStatus)
                {
                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_SUCCESS://成功读取到数据，处理完本次数据后需调用next
                        ProcessSetFaceData(ref struStatus, ref Flag);
                        break;
                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_NEED_WAIT:
                        break;
                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_FAILED:
                        CHCNetSDK.NET_DVR_StopRemoteConfig(m_lSetFaceCfgHandle);
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_FAILED" + CHCNetSDK.NET_DVR_GetLastError().ToString());
                        Flag = false;
                        break;
                    case CHCNetSDK.NET_SDK_GET_NEXT_STATUS_FINISH:
                        CHCNetSDK.NET_DVR_StopRemoteConfig(m_lSetFaceCfgHandle);
                        Flag = false;
                        break;
                    default:
                        MessageBox.Show("NET_SDK_GET_NEXT_STATUS_UNKOWN" + CHCNetSDK.NET_DVR_GetLastError().ToString());
                        Flag = false;
                        CHCNetSDK.NET_DVR_StopRemoteConfig(m_lSetFaceCfgHandle);
                        break;
                }
            }

            Marshal.FreeHGlobal(ptrstruCond);
            Marshal.FreeHGlobal(ptrOutDataLen);
        }
        private void ReadFaceData(ref CHCNetSDK.NET_DVR_FACE_RECORD struRecord)
        {
            if (!File.Exists(facePath))
            {
                MessageBox.Show("The face picture does not exist!");
                return;
            }
            FileStream fs = new FileStream(facePath, FileMode.OpenOrCreate);
            if (0 == fs.Length)
            {
                MessageBox.Show("The face picture is 0k,please input another picture!");
                return;
            }
            if (200 * 1024 < fs.Length)
            {
                MessageBox.Show("The face picture is larger than 200k,please input another picture!");
                return;
            }
            try
            {
                int.TryParse(fs.Length.ToString(), out struRecord.dwFaceLen);
                int iLen = struRecord.dwFaceLen;
                byte[] by = new byte[iLen];
                struRecord.pFaceBuffer = Marshal.AllocHGlobal(iLen);
                fs.Read(by, 0, iLen);
                Marshal.Copy(by, 0, struRecord.pFaceBuffer, iLen);
                fs.Close();
                facePath = "";
            }
            catch
            {
                MessageBox.Show("Read Face Data failed");
                fs.Close();
                return;
            }
        }
        private void ProcessSetFaceData(ref CHCNetSDK.NET_DVR_FACE_STATUS struStatus, ref bool flag)
        {
            switch (struStatus.byRecvStatus)
            {
                case 1:
                    MessageBox.Show("SetFaceDataSuccessful");
                    break;
                default:
                    flag = false;
                    MessageBox.Show("NET_SDK_SET_Face_DATA_FAILED" + struStatus.byRecvStatus.ToString());
                    break;
            }
        }

        // 根据卡号删除人脸
        public void deleteFaceByCardNo(string cardNo)
        {
            facePath = "";
            CHCNetSDK.NET_DVR_FACE_PARAM_CTRL_CARDNO struCardNo = new CHCNetSDK.NET_DVR_FACE_PARAM_CTRL_CARDNO();
            struCardNo.init();
            struCardNo.dwSize = Marshal.SizeOf(struCardNo);
            struCardNo.byMode = 0;
            int dwSize = struCardNo.dwSize;
            byte[] byCardNo = System.Text.Encoding.UTF8.GetBytes(cardNo);
            for (int i = 0; i < byCardNo.Length; i++)
            {
                struCardNo.struByCard.byCardNo[i] = byCardNo[i];
            }

            int dwEnableReaderNo = 1;
            int.TryParse("1", out dwEnableReaderNo);
            if (dwEnableReaderNo <= 0) dwEnableReaderNo = 1;

            struCardNo.struByCard.byEnableCardReader[dwEnableReaderNo - 1] = 1;

            for (int i = 0; i < CHCNetSDK.MAX_FACE_NUM; ++i)
            {
                struCardNo.struByCard.byFaceID[i] = 1;//全部写1删除人脸
            }

            if (false == CHCNetSDK.NET_DVR_RemoteControl(m_UserID, CHCNetSDK.NET_DVR_DEL_FACE_PARAM_CFG, ref struCardNo, dwSize))
            {
                MessageBox.Show("NET_SDK_DEL_FACE_FAILED" + CHCNetSDK.NET_DVR_GetLastError().ToString());
            }
            else
            {
                MessageBox.Show("NET_SDK_DEL_FACE_SUCCEED");
            }
        }
    }
}
