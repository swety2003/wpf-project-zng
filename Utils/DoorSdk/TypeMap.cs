using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project.Utils.DoorSdk
{
    class TypeMap
    {
        public static void AlarmMinorTypeMap(CHCNetSDK.NET_DVR_LOG_V30 stLogInfo, char[] csTmp)
        {
            String szTemp = null;
            switch (stLogInfo.dwMinorType)
            {
                //alarm
                case CHCNetSDK.MINOR_ALARMIN_SHORT_CIRCUIT:
                    szTemp = String.Format("MINOR_ALARMIN_SHORT_CIRCUIT");
                    break;
                case CHCNetSDK.MINOR_ALARMIN_BROKEN_CIRCUIT:
                    szTemp = String.Format("MINOR_ALARMIN_BROKEN_CIRCUIT");
                    break;
                case CHCNetSDK.MINOR_ALARMIN_EXCEPTION:
                    szTemp = String.Format("MINOR_ALARMIN_EXCEPTION");
                    break;
                case CHCNetSDK.MINOR_ALARMIN_RESUME:
                    szTemp = String.Format("MINOR_ALARMIN_RESUME");
                    break;
                case CHCNetSDK.MINOR_HOST_DESMANTLE_ALARM:
                    szTemp = String.Format("MINOR_HOST_DESMANTLE_ALARM");
                    break;
                case CHCNetSDK.MINOR_HOST_DESMANTLE_RESUME:
                    szTemp = String.Format("MINOR_HOST_DESMANTLE_RESUME");
                    break;
                case CHCNetSDK.MINOR_CARD_READER_DESMANTLE_ALARM:
                    szTemp = String.Format("MINOR_CARD_READER_DESMANTLE_ALARM");
                    break;
                case CHCNetSDK.MINOR_CARD_READER_DESMANTLE_RESUME:
                    szTemp = String.Format("MINOR_CARD_READER_DESMANTLE_RESUME");
                    break;
                case CHCNetSDK.MINOR_CASE_SENSOR_ALARM:
                    szTemp = String.Format("MINOR_CASE_SENSOR_ALARM");
                    break;
                case CHCNetSDK.MINOR_CASE_SENSOR_RESUME:
                    szTemp = String.Format("MINOR_CASE_SENSOR_RESUME");
                    break;
                case CHCNetSDK.MINOR_STRESS_ALARM:
                    szTemp = String.Format("MINOR_STRESS_ALARM");
                    break;
                case CHCNetSDK.MINOR_OFFLINE_ECENT_NEARLY_FULL:
                    szTemp = String.Format("MINOR_OFFLINE_ECENT_NEARLY_FULL");
                    break;
                case CHCNetSDK.MINOR_CARD_MAX_AUTHENTICATE_FAIL:
                    szTemp = String.Format("MINOR_CARD_MAX_AUTHENTICATE_FAIL");
                    break;
                case CHCNetSDK.MINOR_SD_CARD_FULL:
                    szTemp = String.Format("MINOR_SD_CARD_FULL");
                    break;
                case CHCNetSDK.MINOR_LINKAGE_CAPTURE_PIC:
                    szTemp = String.Format("MINOR_LINKAGE_CAPTURE_PIC");
                    break;
                case CHCNetSDK.MINOR_SECURITY_MODULE_DESMANTLE_ALARM:
                    szTemp = String.Format("MINOR_SECURITY_MODULE_DESMANTLE_ALARM");
                    break;
                case CHCNetSDK.MINOR_SECURITY_MODULE_DESMANTLE_RESUME:
                    szTemp = String.Format("MINOR_SECURITY_MODULE_DESMANTLE_RESUME");
                    break;
                case CHCNetSDK.MINOR_POS_START_ALARM:
                    szTemp = String.Format("MINOR_POS_START_ALARM");
                    break;
                case CHCNetSDK.MINOR_POS_END_ALARM:
                    szTemp = String.Format("MINOR_POS_END_ALARM");
                    break;
                case CHCNetSDK.MINOR_FACE_IMAGE_QUALITY_LOW:
                    szTemp = String.Format("MINOR_FACE_IMAGE_QUALITY_LOW");
                    break;
                case CHCNetSDK.MINOR_FINGE_RPRINT_QUALITY_LOW:
                    szTemp = String.Format("MINOR_FINGE_RPRINT_QUALITY_LOW");
                    break;
                case CHCNetSDK.MINOR_FIRE_IMPORT_SHORT_CIRCUIT:
                    szTemp = String.Format("MINOR_FIRE_IMPORT_SHORT_CIRCUIT");
                    break;
                case CHCNetSDK.MINOR_FIRE_IMPORT_BROKEN_CIRCUIT:
                    szTemp = String.Format("MINOR_FIRE_IMPORT_BROKEN_CIRCUIT");
                    break;
                case CHCNetSDK.MINOR_FIRE_IMPORT_RESUME:
                    szTemp = String.Format("MINOR_FIRE_IMPORT_RESUME");
                    break;
                case CHCNetSDK.MINOR_FIRE_BUTTON_TRIGGER:
                    szTemp = String.Format("MINOR_FIRE_BUTTON_TRIGGER");
                    break;
                case CHCNetSDK.MINOR_FIRE_BUTTON_RESUME:
                    szTemp = String.Format("MINOR_FIRE_BUTTON_RESUME");
                    break;
                case CHCNetSDK.MINOR_MAINTENANCE_BUTTON_TRIGGER:
                    szTemp = String.Format("MINOR_MAINTENANCE_BUTTON_TRIGGER");
                    break;
                case CHCNetSDK.MINOR_MAINTENANCE_BUTTON_RESUME:
                    szTemp = String.Format("MINOR_MAINTENANCE_BUTTON_RESUME");
                    break;
                case CHCNetSDK.MINOR_EMERGENCY_BUTTON_TRIGGER:
                    szTemp = String.Format("MINOR_EMERGENCY_BUTTON_TRIGGER");
                    break;
                case CHCNetSDK.MINOR_EMERGENCY_BUTTON_RESUME:
                    szTemp = String.Format("MINOR_EMERGENCY_BUTTON_RESUME");
                    break;
                case CHCNetSDK.MINOR_DISTRACT_CONTROLLER_ALARM:
                    szTemp = String.Format("MINOR_DISTRACT_CONTROLLER_ALARM");
                    break;
                case CHCNetSDK.MINOR_DISTRACT_CONTROLLER_RESUME:
                    szTemp = String.Format("MINOR_DISTRACT_CONTROLLER_RESUME");
                    break;
                default:
                    szTemp = String.Format("0x" + stLogInfo.dwMinorType);
                    break;
            }
            szTemp.CopyTo(0, csTmp, 0, szTemp.Length);
            return;
        }

        public static void OperationMinorTypeMap(CHCNetSDK.NET_DVR_LOG_V30 stLogInfo, char[] csTmp)
        {
            String szTemp = null;
            char[] csParaType = new char[256];
            switch (stLogInfo.dwMinorType)
            {
                //operation
                case CHCNetSDK.MINOR_LOCAL_UPGRADE:
                    szTemp = String.Format("MINOR_LOCAL_UPGRADE");
                    break;
                case CHCNetSDK.MINOR_REMOTE_LOGIN:
                    szTemp = String.Format("REMOTE_LOGIN");
                    break;
                case CHCNetSDK.MINOR_REMOTE_LOGOUT:
                    szTemp = String.Format("REMOTE_LOGOUT");
                    break;
                case CHCNetSDK.MINOR_REMOTE_ARM:
                    szTemp = String.Format("REMOTE_ARM");
                    break;
                case CHCNetSDK.MINOR_REMOTE_DISARM:
                    szTemp = String.Format("REMOTE_DISARM");
                    break;
                case CHCNetSDK.MINOR_REMOTE_REBOOT:
                    szTemp = String.Format("REMOTE_REBOOT");
                    break;
                case CHCNetSDK.MINOR_REMOTE_UPGRADE:
                    szTemp = String.Format("REMOTE_UPGRADE");
                    break;
                case CHCNetSDK.MINOR_REMOTE_CFGFILE_OUTPUT:
                    szTemp = String.Format("REMOTE_CFGFILE_OUTPUT");
                    break;
                case CHCNetSDK.MINOR_REMOTE_CFGFILE_INTPUT:
                    szTemp = String.Format("REMOTE_CFGFILE_INTPUT");
                    break;
                case CHCNetSDK.MINOR_REMOTE_ALARMOUT_OPEN_MAN:
                    szTemp = String.Format("MINOR_REMOTE_ALARMOUT_OPEN_MAN");
                    break;
                case CHCNetSDK.MINOR_REMOTE_ALARMOUT_CLOSE_MAN:
                    szTemp = String.Format("MINOR_REMOTE_ALARMOUT_CLOSE_MAN");
                    break;
                case CHCNetSDK.MINOR_REMOTE_OPEN_DOOR:
                    szTemp = String.Format("MINOR_REMOTE_OPEN_DOOR");
                    break;
                case CHCNetSDK.MINOR_REMOTE_CLOSE_DOOR:
                    szTemp = String.Format("MINOR_REMOTE_CLOSE_DOOR");
                    break;
                case CHCNetSDK.MINOR_REMOTE_ALWAYS_OPEN:
                    szTemp = String.Format("MINOR_REMOTE_ALWAYS_OPEN");
                    break;
                case CHCNetSDK.MINOR_REMOTE_ALWAYS_CLOSE:
                    szTemp = String.Format("MINOR_REMOTE_ALWAYS_CLOSE");
                    break;
                case CHCNetSDK.MINOR_REMOTE_CHECK_TIME:
                    szTemp = String.Format("MINOR_REMOTE_CHECK_TIME");
                    break;
                case CHCNetSDK.MINOR_NTP_CHECK_TIME:
                    szTemp = String.Format("MINOR_NTP_CHECK_TIME");
                    break;
                case CHCNetSDK.MINOR_REMOTE_CLEAR_CARD:
                    szTemp = String.Format("MINOR_REMOTE_CLEAR_CARD"); ;
                    break;
                case CHCNetSDK.MINOR_REMOTE_RESTORE_CFG:
                    szTemp = String.Format("MINOR_REMOTE_RESTORE_CFG");
                    break;
                case CHCNetSDK.MINOR_ALARMIN_ARM:
                    szTemp = String.Format("MINOR_ALARMIN_ARM");
                    break;
                case CHCNetSDK.MINOR_ALARMIN_DISARM:
                    szTemp = String.Format("MINOR_ALARMIN_DISARM");
                    break;
                case CHCNetSDK.MINOR_LOCAL_RESTORE_CFG:
                    szTemp = String.Format("MINOR_LOCAL_RESTORE_CFG");
                    break;
                case CHCNetSDK.MINOR_MOD_NET_REPORT_CFG:
                    szTemp = String.Format("MINOR_MOD_NET_REPORT_CFG");
                    break;
                case CHCNetSDK.MINOR_MOD_GPRS_REPORT_PARAM:
                    szTemp = String.Format("MINOR_MOD_GPRS_REPORT_PARAM");
                    break;
                case CHCNetSDK.MINOR_MOD_REPORT_GROUP_PARAM:
                    szTemp = String.Format("MINOR_MOD_REPORT_GROUP_PARAM");
                    break;
                case CHCNetSDK.MINOR_UNLOCK_PASSWORD_OPEN_DOOR:
                    szTemp = String.Format("MINOR_UNLOCK_PASSWORD_OPEN_DOOR");
                    break;
                case CHCNetSDK.MINOR_REMOTE_CAPTURE_PIC:
                    szTemp = String.Format("MINOR_REMOTE_CAPTURE_PIC"); ;
                    break;
                case CHCNetSDK.MINOR_AUTO_RENUMBER:
                    szTemp = String.Format("MINOR_AUTO_RENUMBER");
                    break;
                case CHCNetSDK.MINOR_AUTO_COMPLEMENT_NUMBER:
                    szTemp = String.Format("MINOR_AUTO_COMPLEMENT_NUMBER");
                    break;
                case CHCNetSDK.MINOR_NORMAL_CFGFILE_INPUT:
                    szTemp = String.Format("MINOR_NORMAL_CFGFILE_INPUT");
                    break;
                case CHCNetSDK.MINOR_NORMAL_CFGFILE_OUTTPUT:
                    szTemp = String.Format("MINOR_NORMAL_CFGFILE_OUTTPUT");
                    break;
                case CHCNetSDK.MINOR_CARD_RIGHT_INPUT:
                    szTemp = String.Format("MINOR_CARD_RIGHT_INPUT");
                    break;
                case CHCNetSDK.MINOR_CARD_RIGHT_OUTTPUT:
                    szTemp = String.Format("MINOR_CARD_RIGHT_OUTTPUT");
                    break;
                case CHCNetSDK.MINOR_LOCAL_USB_UPGRADE:
                    szTemp = String.Format("MINOR_LOCAL_USB_UPGRADE");
                    break;
                case CHCNetSDK.MINOR_REMOTE_VISITOR_CALL_LADDER:
                    szTemp = String.Format("MINOR_REMOTE_VISITOR_CALL_LADDER"); ;
                    break;
                case CHCNetSDK.MINOR_REMOTE_HOUSEHOLD_CALL_LADDER:
                    szTemp = String.Format("MINOR_REMOTE_HOUSEHOLD_CALL_LADDER"); ;
                    break;
                default:
                    szTemp = String.Format("0x" + stLogInfo.dwMinorType);
                    break;
            }
            szTemp.CopyTo(0, csTmp, 0, szTemp.Length);
            return;
        }

        public static void ExceptionMinorTypeMap(CHCNetSDK.NET_DVR_LOG_V30 stLogInfo, char[] csTmp)
        {
            String szTemp = null;
            switch (stLogInfo.dwMinorType)
            {
                //exception
                case CHCNetSDK.MINOR_NET_BROKEN:
                    szTemp = String.Format("MINOR_NET_BROKEN");
                    break;
                case CHCNetSDK.MINOR_RS485_DEVICE_ABNORMAL:
                    szTemp = String.Format("MINOR_RS485_DEVICE_ABNORMAL");
                    break;
                case CHCNetSDK.MINOR_RS485_DEVICE_REVERT:
                    szTemp = String.Format("MINOR_RS485_DEVICE_REVERT");
                    break;
                case CHCNetSDK.MINOR_DEV_POWER_ON:
                    szTemp = String.Format("MINOR_DEV_POWER_ON");
                    break;
                case CHCNetSDK.MINOR_DEV_POWER_OFF:
                    szTemp = String.Format("MINOR_DEV_POWER_OFF");
                    break;
                case CHCNetSDK.MINOR_WATCH_DOG_RESET:
                    szTemp = String.Format("MINOR_WATCH_DOG_RESET");
                    break;
                case CHCNetSDK.MINOR_LOW_BATTERY:
                    szTemp = String.Format("MINOR_LOW_BATTERY");
                    break;
                case CHCNetSDK.MINOR_BATTERY_RESUME:
                    szTemp = String.Format("MINOR_BATTERY_RESUME");
                    break;
                case CHCNetSDK.MINOR_AC_OFF:
                    szTemp = String.Format("MINOR_AC_OFF");
                    break;
                case CHCNetSDK.MINOR_AC_RESUME:
                    szTemp = String.Format("MINOR_AC_RESUME");
                    break;
                case CHCNetSDK.MINOR_NET_RESUME:
                    szTemp = String.Format("MINOR_NET_RESUME");
                    break;
                case CHCNetSDK.MINOR_FLASH_ABNORMAL:
                    szTemp = String.Format("MINOR_FLASH_ABNORMAL");
                    break;
                case CHCNetSDK.MINOR_CARD_READER_OFFLINE:
                    szTemp = String.Format("MINOR_CARD_READER_OFFLINE");
                    break;
                case CHCNetSDK.MINOR_CARD_READER_RESUME:
                    szTemp = String.Format("MINOR_CAED_READER_RESUME");
                    break;
                case CHCNetSDK.MINOR_INDICATOR_LIGHT_OFF:
                    szTemp = String.Format("MINOR_INDICATOR_LIGHT_OFF");
                    break;
                case CHCNetSDK.MINOR_INDICATOR_LIGHT_RESUME:
                    szTemp = String.Format("MINOR_INDICATOR_LIGHT_RESUME");
                    break;
                case CHCNetSDK.MINOR_CHANNEL_CONTROLLER_OFF:
                    szTemp = String.Format("MINOR_CHANNEL_CONTROLLER_OFF");
                    break;
                case CHCNetSDK.MINOR_CHANNEL_CONTROLLER_RESUME:
                    szTemp = String.Format("MINOR_CHANNEL_CONTROLLER_RESUME");
                    break;
                case CHCNetSDK.MINOR_SECURITY_MODULE_OFF:
                    szTemp = String.Format("MINOR_SECURITY_MODULE_OFF");
                    break;
                case CHCNetSDK.MINOR_SECURITY_MODULE_RESUME:
                    szTemp = String.Format("MINOR_SECURITY_MODULE_RESUME");
                    break;
                case CHCNetSDK.MINOR_BATTERY_ELECTRIC_LOW:
                    szTemp = String.Format("MINOR_BATTERY_ELECTRIC_LOW");
                    break;
                case CHCNetSDK.MINOR_BATTERY_ELECTRIC_RESUME:
                    szTemp = String.Format("MINOR_BATTERY_ELECTRIC_RESUME");
                    break;
                case CHCNetSDK.MINOR_LOCAL_CONTROL_NET_BROKEN:
                    szTemp = String.Format("MINOR_LOCAL_CONTROL_NET_BROKEN");
                    break;
                case CHCNetSDK.MINOR_LOCAL_CONTROL_NET_RSUME:
                    szTemp = String.Format("MINOR_LOCAL_CONTROL_NET_RSUME");
                    break;
                case CHCNetSDK.MINOR_MASTER_RS485_LOOPNODE_BROKEN:
                    szTemp = String.Format("MINOR_MASTER_RS485_LOOPNODE_BROKEN");
                    break;
                case CHCNetSDK.MINOR_MASTER_RS485_LOOPNODE_RESUME:
                    szTemp = String.Format("MINOR_MASTER_RS485_LOOPNODE_RESUME");
                    break;
                case CHCNetSDK.MINOR_LOCAL_CONTROL_OFFLINE:
                    szTemp = String.Format("MINOR_LOCAL_CONTROL_OFFLINE");
                    break;
                case CHCNetSDK.MINOR_LOCAL_CONTROL_RESUME:
                    szTemp = String.Format("MINOR_LOCAL_CONTROL_RESUME");
                    break;
                case CHCNetSDK.MINOR_LOCAL_DOWNSIDE_RS485_LOOPNODE_BROKEN:
                    szTemp = String.Format("MINOR_LOCAL_DOWNSIDE_RS485_LOOPNODE_BROKEN");
                    break;
                case CHCNetSDK.MINOR_LOCAL_DOWNSIDE_RS485_LOOPNODE_RESUME:
                    szTemp = String.Format("MINOR_LOCAL_DOWNSIDE_RS485_LOOPNODE_RESUME");
                    break;
                case CHCNetSDK.MINOR_DISTRACT_CONTROLLER_ONLINE:
                    szTemp = String.Format("MINOR_DISTRACT_CONTROLLER_ONLINE");
                    break;
                case CHCNetSDK.MINOR_DISTRACT_CONTROLLER_OFFLINE:
                    szTemp = String.Format("MINOR_DISTRACT_CONTROLLER_OFFLINE");
                    break;
                case CHCNetSDK.MINOR_ID_CARD_READER_NOT_CONNECT:
                    szTemp = String.Format("MINOR_ID_CARD_READER_NOT_CONNECT");
                    break;
                case CHCNetSDK.MINOR_ID_CARD_READER_RESUME:
                    szTemp = String.Format("MINOR_ID_CARD_READER_RESUME");
                    break;
                case CHCNetSDK.MINOR_FINGER_PRINT_MODULE_NOT_CONNECT:
                    szTemp = String.Format("MINOR_FINGER_PRINT_MODULE_NOT_CONNECT");
                    break;
                case CHCNetSDK.MINOR_FINGER_PRINT_MODULE_RESUME:
                    szTemp = String.Format("MINOR_FINGER_PRINT_MODULE_RESUME");
                    break;
                case CHCNetSDK.MINOR_CAMERA_NOT_CONNECT:
                    szTemp = String.Format("MINOR_CAMERA_NOT_CONNECT");
                    break;
                case CHCNetSDK.MINOR_CAMERA_RESUME:
                    szTemp = String.Format("MINOR_CAMERA_RESUME");
                    break;
                case CHCNetSDK.MINOR_COM_NOT_CONNECT:
                    szTemp = String.Format("MINOR_COM_NOT_CONNECT");
                    break;
                case CHCNetSDK.MINOR_COM_RESUME:
                    szTemp = String.Format("MINOR_COM_RESUME");
                    break;
                case CHCNetSDK.MINOR_DEVICE_NOT_AUTHORIZE:
                    szTemp = String.Format("MINOR_DEVICE_NOT_AUTHORIZE");
                    break;
                case CHCNetSDK.MINOR_PEOPLE_AND_ID_CARD_DEVICE_ONLINE:
                    szTemp = String.Format("MINOR_PEOPLE_AND_ID_CARD_DEVICE_ONLINE");
                    break;
                case CHCNetSDK.MINOR_PEOPLE_AND_ID_CARD_DEVICE_OFFLINE:
                    szTemp = String.Format("MINOR_PEOPLE_AND_ID_CARD_DEVICE_OFFLINE");
                    break;
                case CHCNetSDK.MINOR_LOCAL_LOGIN_LOCK:
                    szTemp = String.Format("MINOR_LOCAL_LOGIN_LOCK");
                    break;
                case CHCNetSDK.MINOR_LOCAL_LOGIN_UNLOCK:
                    szTemp = String.Format("MINOR_LOCAL_LOGIN_UNLOCK");
                    break;
                default:
                    szTemp = String.Format("0x" + stLogInfo.dwMinorType);
                    break;
            }
            szTemp.CopyTo(0, csTmp, 0, szTemp.Length);
            return;
        }

        public static void EventMinorTypeMap(CHCNetSDK.NET_DVR_LOG_V30 stLogInfo, char[] csTmp)
        {
            String szTemp = null;
            switch (stLogInfo.dwMinorType)
            {
                case CHCNetSDK.MINOR_LEGAL_CARD_PASS:
                    szTemp = String.Format("MINOR_LEGAL_CARD_PASS");
                    break;
                case CHCNetSDK.MINOR_CARD_AND_PSW_PASS:
                    szTemp = String.Format("MINOR_CARD_AND_PSW_PASS");
                    break;
                case CHCNetSDK.MINOR_CARD_AND_PSW_FAIL:
                    szTemp = String.Format("MINOR_CARD_AND_PSW_FAIL");
                    break;
                case CHCNetSDK.MINOR_CARD_AND_PSW_TIMEOUT:
                    szTemp = String.Format("MINOR_CARD_AND_PSW_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_CARD_AND_PSW_OVER_TIME:
                    szTemp = String.Format("MINOR_CARD_AND_PSW_OVER_TIME");
                    break;
                case CHCNetSDK.MINOR_CARD_NO_RIGHT:
                    szTemp = String.Format("MINOR_CARD_NO_RIGHT");
                    break;
                case CHCNetSDK.MINOR_CARD_INVALID_PERIOD:
                    szTemp = String.Format("MINOR_CARD_INVALID_PERIOD");
                    break;
                case CHCNetSDK.MINOR_CARD_OUT_OF_DATE:
                    szTemp = String.Format("MINOR_CARD_OUT_OF_DATE");
                    break;
                case CHCNetSDK.MINOR_INVALID_CARD:
                    szTemp = String.Format("MINOR_INVALID_CARD");
                    break;
                case CHCNetSDK.MINOR_ANTI_SNEAK_FAIL:
                    szTemp = String.Format("MINOR_ANTI_SNEAK_FAIL");
                    break;
                case CHCNetSDK.MINOR_INTERLOCK_DOOR_NOT_CLOSE:
                    szTemp = String.Format("MINOR_INTERLOCK_DOOR_NOT_CLOSE");
                    break;
                case CHCNetSDK.MINOR_NOT_BELONG_MULTI_GROUP:
                    szTemp = String.Format("MINOR_NOT_BELONG_MULTI_GROUP");
                    break;
                case CHCNetSDK.MINOR_INVALID_MULTI_VERIFY_PERIOD:
                    szTemp = String.Format("MINOR_INVALID_MULTI_VERIFY_PERIOD");
                    break;
                case CHCNetSDK.MINOR_MULTI_VERIFY_SUPER_RIGHT_FAIL:
                    szTemp = String.Format("MINOR_MULTI_VERIFY_SUPER_RIGHT_FAIL");
                    break;
                case CHCNetSDK.MINOR_MULTI_VERIFY_REMOTE_RIGHT_FAIL:
                    szTemp = String.Format("MINOR_MULTI_VERIFY_REMOTE_RIGHT_FAIL");
                    break;
                case CHCNetSDK.MINOR_MULTI_VERIFY_SUCCESS:
                    szTemp = String.Format("MINOR_MULTI_VERIFY_SUCCESS");
                    break;
                case CHCNetSDK.MINOR_LEADER_CARD_OPEN_BEGIN:
                    szTemp = String.Format("MINOR_LEADER_CARD_OPEN_BEGIN");
                    break;
                case CHCNetSDK.MINOR_LEADER_CARD_OPEN_END:
                    szTemp = String.Format("MINOR_LEADER_CARD_OPEN_END");
                    break;
                case CHCNetSDK.MINOR_ALWAYS_OPEN_BEGIN:
                    szTemp = String.Format("MINOR_ALWAYS_OPEN_BEGIN");
                    break;
                case CHCNetSDK.MINOR_ALWAYS_OPEN_END:
                    szTemp = String.Format("MINOR_ALWAYS_OPEN_END");
                    break;
                case CHCNetSDK.MINOR_LOCK_OPEN:
                    szTemp = String.Format("MINOR_LOCK_OPEN");
                    break;
                case CHCNetSDK.MINOR_LOCK_CLOSE:
                    szTemp = String.Format("MINOR_LOCK_CLOSE");
                    break;
                case CHCNetSDK.MINOR_DOOR_BUTTON_PRESS:
                    szTemp = String.Format("MINOR_DOOR_BUTTON_PRESS");
                    break;
                case CHCNetSDK.MINOR_DOOR_BUTTON_RELEASE:
                    szTemp = String.Format("MINOR_DOOR_BUTTON_RELEASE");
                    break;
                case CHCNetSDK.MINOR_DOOR_OPEN_NORMAL:
                    szTemp = String.Format("MINOR_DOOR_OPEN_NORMAL");
                    break;
                case CHCNetSDK.MINOR_DOOR_CLOSE_NORMAL:
                    szTemp = String.Format("MINOR_DOOR_CLOSE_NORMAL");
                    break;
                case CHCNetSDK.MINOR_DOOR_OPEN_ABNORMAL:
                    szTemp = String.Format("MINOR_DOOR_OPEN_ABNORMAL");
                    break;
                case CHCNetSDK.MINOR_DOOR_OPEN_TIMEOUT:
                    szTemp = String.Format("MINOR_DOOR_OPEN_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_ALARMOUT_ON:
                    szTemp = String.Format("MINOR_ALARMOUT_ON");
                    break;
                case CHCNetSDK.MINOR_ALARMOUT_OFF:
                    szTemp = String.Format("MINOR_ALARMOUT_OFF");
                    break;
                case CHCNetSDK.MINOR_ALWAYS_CLOSE_BEGIN:
                    szTemp = String.Format("MINOR_ALWAYS_CLOSE_BEGIN");
                    break;
                case CHCNetSDK.MINOR_ALWAYS_CLOSE_END:
                    szTemp = String.Format("MINOR_ALWAYS_CLOSE_END");
                    break;
                case CHCNetSDK.MINOR_MULTI_VERIFY_NEED_REMOTE_OPEN:
                    szTemp = String.Format("MINOR_MULTI_VERIFY_NEED_REMOTE_OPEN");
                    break;
                case CHCNetSDK.MINOR_MULTI_VERIFY_SUPERPASSWD_VERIFY_SUCCESS:
                    szTemp = String.Format("MINOR_MULTI_VERIFY_SUPERPASSWD_VERIFY_SUCCESS");
                    break;
                case CHCNetSDK.MINOR_MULTI_VERIFY_REPEAT_VERIFY:
                    szTemp = String.Format("MINOR_MULTI_VERIFY_REPEAT_VERIFY");
                    break;
                case CHCNetSDK.MINOR_MULTI_VERIFY_TIMEOUT:
                    szTemp = String.Format("MINOR_MULTI_VERIFY_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_DOORBELL_RINGING:
                    szTemp = String.Format("MINOR_DOORBELL_RINGING");
                    break;
                case CHCNetSDK.MINOR_FINGERPRINT_COMPARE_PASS:
                    szTemp = String.Format("MINOR_FINGERPRINT_COMPARE_PASS");
                    break;
                case CHCNetSDK.MINOR_FINGERPRINT_COMPARE_FAIL:
                    szTemp = String.Format("MINOR_FINGERPRINT_COMPARE_FAIL");
                    break;
                case CHCNetSDK.MINOR_CARD_FINGERPRINT_VERIFY_PASS:
                    szTemp = String.Format("MINOR_CARD_FINGERPRINT_VERIFY_PASS");
                    break;
                case CHCNetSDK.MINOR_CARD_FINGERPRINT_VERIFY_FAIL:
                    szTemp = String.Format("MINOR_CARD_FINGERPRINT_VERIFY_FAIL");
                    break;
                case CHCNetSDK.MINOR_CARD_FINGERPRINT_VERIFY_TIMEOUT:
                    szTemp = String.Format("MINOR_CARD_FINGERPRINT_VERIFY_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_PASS:
                    szTemp = String.Format("MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_PASS");
                    break;
                case CHCNetSDK.MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_FAIL:
                    szTemp = String.Format("MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_FAIL");
                    break;
                case CHCNetSDK.MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_TIMEOUT:
                    szTemp = String.Format("MINOR_CARD_FINGERPRINT_PASSWD_VERIFY_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_FINGERPRINT_PASSWD_VERIFY_PASS:
                    szTemp = String.Format("MINOR_FINGERPRINT_PASSWD_VERIFY_PASS");
                    break;
                case CHCNetSDK.MINOR_FINGERPRINT_PASSWD_VERIFY_FAIL:
                    szTemp = String.Format("MINOR_FINGERPRINT_PASSWD_VERIFY_FAIL");
                    break;
                case CHCNetSDK.MINOR_FINGERPRINT_PASSWD_VERIFY_TIMEOUT:
                    szTemp = String.Format("MINOR_FINGERPRINT_PASSWD_VERIFY_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_FINGERPRINT_INEXISTENCE:
                    szTemp = String.Format("MINOR_FINGERPRINT_INEXISTENCE");
                    break;
                case CHCNetSDK.MINOR_CARD_PLATFORM_VERIFY:
                    szTemp = String.Format("MINOR_CARD_PLATFORM_VERIFY");
                    break;
                case CHCNetSDK.MINOR_CALL_CENTER:
                    szTemp = String.Format("MINOR_CALL_CENTER");
                    break;
                case CHCNetSDK.MINOR_FIRE_RELAY_TURN_ON_DOOR_ALWAYS_OPEN:
                    szTemp = String.Format("MINOR_FIRE_RELAY_TURN_ON_DOOR_ALWAYS_OPEN");
                    break;
                case CHCNetSDK.MINOR_FIRE_RELAY_RECOVER_DOOR_RECOVER_NORMAL:
                    szTemp = String.Format("MINOR_FIRE_RELAY_RECOVER_DOOR_RECOVER_NORMAL");
                    break;
                case CHCNetSDK.MINOR_FACE_AND_FP_VERIFY_PASS:
                    szTemp = String.Format("MINOR_FACE_AND_FP_VERIFY_PASS");
                    break;
                case CHCNetSDK.MINOR_FACE_AND_FP_VERIFY_FAIL:
                    szTemp = String.Format("MINOR_FACE_AND_FP_VERIFY_FAIL");
                    break;
                case CHCNetSDK.MINOR_FACE_AND_FP_VERIFY_TIMEOUT:
                    szTemp = String.Format("MINOR_FACE_AND_FP_VERIFY_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_FACE_AND_PW_VERIFY_PASS:
                    szTemp = String.Format("MINOR_FACE_AND_PW_VERIFY_PASS");
                    break;
                case CHCNetSDK.MINOR_FACE_AND_PW_VERIFY_FAIL:
                    szTemp = String.Format("MINOR_FACE_AND_PW_VERIFY_FAIL");
                    break;
                case CHCNetSDK.MINOR_FACE_AND_PW_VERIFY_TIMEOUT:
                    szTemp = String.Format("MINOR_FACE_AND_PW_VERIFY_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_FACE_AND_CARD_VERIFY_PASS:
                    szTemp = String.Format("MINOR_FACE_AND_CARD_VERIFY_PASS");
                    break;
                case CHCNetSDK.MINOR_FACE_AND_CARD_VERIFY_FAIL:
                    szTemp = String.Format("MINOR_FACE_AND_CARD_VERIFY_FAIL");
                    break;
                case CHCNetSDK.MINOR_FACE_AND_CARD_VERIFY_TIMEOUT:
                    szTemp = String.Format("MINOR_FACE_AND_CARD_VERIFY_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_FACE_AND_PW_AND_FP_VERIFY_PASS:
                    szTemp = String.Format("MINOR_FACE_AND_PW_AND_FP_VERIFY_PASS");
                    break;
                case CHCNetSDK.MINOR_FACE_AND_PW_AND_FP_VERIFY_FAIL:
                    szTemp = String.Format("MINOR_FACE_AND_PW_AND_FP_VERIFY_FAIL");
                    break;
                case CHCNetSDK.MINOR_FACE_AND_PW_AND_FP_VERIFY_TIMEOUT:
                    szTemp = String.Format("MINOR_FACE_AND_PW_AND_FP_VERIFY_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_FACE_CARD_AND_FP_VERIFY_PASS:
                    szTemp = String.Format("MINOR_FACE_CARD_AND_FP_VERIFY_PASS");
                    break;
                case CHCNetSDK.MINOR_FACE_CARD_AND_FP_VERIFY_FAIL:
                    szTemp = String.Format("MINOR_FACE_CARD_AND_FP_VERIFY_FAIL");
                    break;
                case CHCNetSDK.MINOR_FACE_CARD_AND_FP_VERIFY_TIMEOUT:
                    szTemp = String.Format("MINOR_FACE_CARD_AND_FP_VERIFY_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_EMPLOYEENO_AND_FP_VERIFY_PASS:
                    szTemp = String.Format("MINOR_EMPLOYEENO_AND_FP_VERIFY_PASS");
                    break;
                case CHCNetSDK.MINOR_EMPLOYEENO_AND_FP_VERIFY_FAIL:
                    szTemp = String.Format("MINOR_EMPLOYEENO_AND_FP_VERIFY_FAIL");
                    break;
                case CHCNetSDK.MINOR_EMPLOYEENO_AND_FP_VERIFY_TIMEOUT:
                    szTemp = String.Format("MINOR_EMPLOYEENO_AND_FP_VERIFY_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_EMPLOYEENO_AND_FP_AND_PW_VERIFY_PASS:
                    szTemp = String.Format("MINOR_EMPLOYEENO_AND_FP_AND_PW_VERIFY_PASS");
                    break;
                case CHCNetSDK.MINOR_EMPLOYEENO_AND_FP_AND_PW_VERIFY_FAIL:
                    szTemp = String.Format("MINOR_EMPLOYEENO_AND_FP_AND_PW_VERIFY_FAIL");
                    break;
                case CHCNetSDK.MINOR_EMPLOYEENO_AND_FP_AND_PW_VERIFY_TIMEOUT:
                    szTemp = String.Format("MINOR_EMPLOYEENO_AND_FP_AND_PW_VERIFY_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_FACE_VERIFY_PASS:
                    szTemp = String.Format("MINOR_FACE_VERIFY_PASS");
                    break;
                case CHCNetSDK.MINOR_FACE_VERIFY_FAIL:
                    szTemp = String.Format("MINOR_FACE_VERIFY_FAIL");
                    break;
                case CHCNetSDK.MINOR_EMPLOYEENO_AND_FACE_VERIFY_PASS:
                    szTemp = String.Format("MINOR_EMPLOYEENO_AND_FACE_VERIFY_PASS");
                    break;
                case CHCNetSDK.MINOR_EMPLOYEENO_AND_FACE_VERIFY_FAIL:
                    szTemp = String.Format("MINOR_EMPLOYEENO_AND_FACE_VERIFY_FAIL");
                    break;
                case CHCNetSDK.MINOR_EMPLOYEENO_AND_FACE_VERIFY_TIMEOUT:
                    szTemp = String.Format("MINOR_EMPLOYEENO_AND_FACE_VERIFY_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_FACE_RECOGNIZE_FAIL:
                    szTemp = String.Format("MINOR_FACE_RECOGNIZE_FAIL");
                    break;
                case CHCNetSDK.MINOR_FIRSTCARD_AUTHORIZE_BEGIN:
                    szTemp = String.Format("MINOR_FIRSTCARD_AUTHORIZE_BEGIN");
                    break;
                case CHCNetSDK.MINOR_FIRSTCARD_AUTHORIZE_END:
                    szTemp = String.Format("MINOR_FIRSTCARD_AUTHORIZE_END");
                    break;
                case CHCNetSDK.MINOR_DOORLOCK_INPUT_SHORT_CIRCUIT:
                    szTemp = String.Format("MINOR_DOORLOCK_INPUT_SHORT_CIRCUIT");
                    break;
                case CHCNetSDK.MINOR_DOORLOCK_INPUT_BROKEN_CIRCUIT:
                    szTemp = String.Format("MINOR_DOORLOCK_INPUT_BROKEN_CIRCUIT");
                    break;
                case CHCNetSDK.MINOR_DOORLOCK_INPUT_EXCEPTION:
                    szTemp = String.Format("MINOR_DOORLOCK_INPUT_EXCEPTION");
                    break;
                case CHCNetSDK.MINOR_DOORCONTACT_INPUT_SHORT_CIRCUIT:
                    szTemp = String.Format("MINOR_DOORCONTACT_INPUT_SHORT_CIRCUIT");
                    break;
                case CHCNetSDK.MINOR_DOORCONTACT_INPUT_BROKEN_CIRCUIT:
                    szTemp = String.Format("MINOR_DOORCONTACT_INPUT_BROKEN_CIRCUIT");
                    break;
                case CHCNetSDK.MINOR_DOORCONTACT_INPUT_EXCEPTION:
                    szTemp = String.Format("MINOR_DOORCONTACT_INPUT_EXCEPTION");
                    break;
                case CHCNetSDK.MINOR_OPENBUTTON_INPUT_SHORT_CIRCUIT:
                    szTemp = String.Format("MINOR_OPENBUTTON_INPUT_SHORT_CIRCUIT");
                    break;
                case CHCNetSDK.MINOR_OPENBUTTON_INPUT_BROKEN_CIRCUIT:
                    szTemp = String.Format("MINOR_OPENBUTTON_INPUT_BROKEN_CIRCUIT");
                    break;
                case CHCNetSDK.MINOR_OPENBUTTON_INPUT_EXCEPTION:
                    szTemp = String.Format("MINOR_OPENBUTTON_INPUT_EXCEPTION");
                    break;
                case CHCNetSDK.MINOR_DOORLOCK_OPEN_EXCEPTION:
                    szTemp = String.Format("MINOR_DOORLOCK_OPEN_EXCEPTION");
                    break;
                case CHCNetSDK.MINOR_DOORLOCK_OPEN_TIMEOUT:
                    szTemp = String.Format("MINOR_DOORLOCK_OPEN_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_FIRSTCARD_OPEN_WITHOUT_AUTHORIZE:
                    szTemp = String.Format("MINOR_FIRSTCARD_OPEN_WITHOUT_AUTHORIZE");
                    break;
                case CHCNetSDK.MINOR_CALL_LADDER_RELAY_BREAK:
                    szTemp = String.Format("MINOR_CALL_LADDER_RELAY_BREAK");
                    break;
                case CHCNetSDK.MINOR_CALL_LADDER_RELAY_CLOSE:
                    szTemp = String.Format("MINOR_CALL_LADDER_RELAY_CLOSE");
                    break;
                case CHCNetSDK.MINOR_AUTO_KEY_RELAY_BREAK:
                    szTemp = String.Format("MINOR_AUTO_KEY_RELAY_BREAK");
                    break;
                case CHCNetSDK.MINOR_AUTO_KEY_RELAY_CLOSE:
                    szTemp = String.Format("MINOR_AUTO_KEY_RELAY_CLOSE");
                    break;
                case CHCNetSDK.MINOR_KEY_CONTROL_RELAY_BREAK:
                    szTemp = String.Format("MINOR_KEY_CONTROL_RELAY_BREAK");
                    break;
                case CHCNetSDK.MINOR_KEY_CONTROL_RELAY_CLOSE:
                    szTemp = String.Format("MINOR_KEY_CONTROL_RELAY_CLOSE");
                    break;
                case CHCNetSDK.MINOR_EMPLOYEENO_AND_PW_PASS:
                    szTemp = String.Format("MINOR_EMPLOYEENO_AND_PW_PASS");
                    break;
                case CHCNetSDK.MINOR_EMPLOYEENO_AND_PW_FAIL:
                    szTemp = String.Format("MINOR_EMPLOYEENO_AND_PW_FAIL");
                    break;
                case CHCNetSDK.MINOR_EMPLOYEENO_AND_PW_TIMEOUT:
                    szTemp = String.Format("MINOR_EMPLOYEENO_AND_PW_TIMEOUT");
                    break;
                case CHCNetSDK.MINOR_HUMAN_DETECT_FAIL:
                    szTemp = String.Format("MINOR_HUMAN_DETECT_FAIL");
                    break;
                case CHCNetSDK.MINOR_PEOPLE_AND_ID_CARD_COMPARE_PASS:
                    szTemp = String.Format("MINOR_PEOPLE_AND_ID_CARD_COMPARE_PASS");
                    break;
                case CHCNetSDK.MINOR_PEOPLE_AND_ID_CARD_COMPARE_FAIL:
                    szTemp = String.Format("MINOR_PEOPLE_AND_ID_CARD_COMPARE_FAIL");
                    break;
                case CHCNetSDK.MINOR_CERTIFICATE_BLACK_LIST:
                    szTemp = String.Format("MINOR_CERTIFICATE_BLACK_LIST");
                    break;
                case CHCNetSDK.MINOR_LEGAL_MESSAGE:
                    szTemp = String.Format("MINOR_LEGAL_MESSAGE");
                    break;
                case CHCNetSDK.MINOR_ILLEGAL_MESSAGE:
                    szTemp = String.Format("MINOR_ILLEGAL_MESSAGE");
                    break;
                case CHCNetSDK.MINOR_MAC_DETECT:
                    szTemp = String.Format("MINOR_MAC_DETECT");
                    break;
                default:
                    szTemp = String.Format("Main Event unknown:" + "0x" + "stLogInfo.dwMinorType");
                    break;
            }
            szTemp.CopyTo(0, csTmp, 0, szTemp.Length);
            return;
        }
    }
}
