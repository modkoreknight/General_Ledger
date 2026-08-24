using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace Interact.BusinessLogic
{
    public static class ServerInfo
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public static String GetMachineCode()
        {
            return ServerInfo.GetCpuID() + ServerInfo.GetVolumeSerial(String.Empty) + ServerInfo.GetMacAddress();
        }

        public static String GetCpuID()
        {
            String cpuID = String.Empty;
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (cpuID == String.Empty)
                {
                    cpuID = mo.Properties["ProcessorId"].Value.ToString();
                }
            }
            //16 characters
            return cpuID;
        }

        public static String GetVolumeSerial(String driveLetter)
        {
            String diskSerial = String.Empty;
            if (String.IsNullOrEmpty(driveLetter))
            {
                driveLetter = "C";
            }
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"" + driveLetter + ":\"");
            disk.Get();
            diskSerial = disk["VolumeSerialNumber"].ToString();
            //8 characters
            return diskSerial;
        }

        public static String GetMacAddress()
        {
            String macAddress = String.Empty;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (macAddress == String.Empty)
                {
                    if ((Boolean)mo["IPEnabled"] == true)
                    {
                        macAddress = mo["MacAddress"].ToString();
                    }
                }
                mo.Dispose();
            }
            macAddress = macAddress.Replace(":", "");
            //12 characters
            return macAddress;
        }
        #endregion
    }
}
