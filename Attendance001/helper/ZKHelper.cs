using Attendance001.objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using zkemkeeper;

namespace Attendance001.helper
{
    class ZKHelper
    {
        private bool bIsConnected = false;//the boolean value identifies whether the device is connected
        private int iMachineNumber = 1;//the serial number of the device.After connecting the device ,this value will be changed.

        //Create Standalone SDK class dynamicly.
        public static CZKEMClass axCZKEM1 = new CZKEMClass();
        private FileHelper fHelper;
        private NetworkDetails netDet;
        
        public ZKHelper()
        {

            fHelper = new FileHelper();
            netDet = fHelper.readNetworkDetails();

            FileHelper.writeToLog("\r\n\n" + "Test Text");
        }

        private bool open()
        {
            bIsConnected = axCZKEM1.Connect_Net(netDet.IP_AM, Convert.ToInt32(netDet.PORT_AM));
            if (bIsConnected == true)
            {
                return true;
            }
            else
            {
                NotificationHelper.CreateNotification("Unable to connect to Machine");
                FileHelper.writeToLog("Unable to connect to Machine");
                return false;
            }
        }

        public List<Record> getAllLogData()
        {

            List<Record> records = new List<Record>();

            if (!open())
            {
                return records;
            }

            int idwErrorCode = 0;

            string sdwEnrollNumber = "";
            int idwVerifyMode = 0;
            int idwInOutMode = 0;
            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;
            int idwSecond = 0;
            int idwWorkcode = 0;

            axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device

            if (axCZKEM1.ReadGeneralLogData(iMachineNumber))//read all the attendance records to the memory
            {
                while (axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode,
                            out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
                {

                    string hour = idwHour.ToString(),
                        min = idwMinute.ToString();

                    if (hour.Length == 1)
                    {
                        hour = "0" + hour;
                    }
                    if (min.Length == 1)
                    {
                        min = "0" + min;
                    }

                    Record record = new Record(int.Parse(sdwEnrollNumber),
                        idwMonth.ToString() + "/" + idwDay.ToString() + "/" + idwYear.ToString(),
                        hour + "." + min);
                    records.Add(record);
                }

                axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device               
                axCZKEM1.Disconnect();

                return records;
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);

                if (idwErrorCode != 0)
                {
                    NotificationHelper.CreateNotification("Reading data from terminal failed,ErrorCode: " + idwErrorCode.ToString());
                    FileHelper.writeToLog("Reading data from terminal failed,ErrorCode: " + idwErrorCode.ToString(), "Error");
                }
                else
                {
                    NotificationHelper.CreateNotification("No data from terminal returns!");
                    FileHelper.writeToLog("No data from terminal returns!", "Error");
                }
            }

            return null;
        }

        public bool deleteLogData()
        {
            if (!open())
            {
                return false;
            }

            try
            {
                axCZKEM1.EnableDevice(iMachineNumber, true);    //enable the device  
                FileHelper.writeToLog("Deleting records");
                //txt_error.Text = "Deleting records";
                //axCZKEM1.DeleteAttlogBetweenTheDate(iMachineNumber, "7/5/2012", "8/5/2012");
                //YYYY - MM - DD hh: mm: ss
                bool ret = axCZKEM1.DeleteAttlogBetweenTheDate(iMachineNumber, "2018-8-29 00:00:00", "2018-8-30 23:59:59");
                //bool ret = axCZKEM1.DeleteAttlogByTime(iMachineNumber, "2018-1-23 00:00:00");
                //bool ret = axCZKEM1.ClearGLog(iMachineNumber);

                if (!ret)
                {
                    int idwErrorCode = 0;
                    axCZKEM1.GetLastError(ref idwErrorCode);
                    NotificationHelper.CreateNotification("Operation failed, ErrorCode = " + idwErrorCode.ToString());
                    FileHelper.writeToLog("Operation failed, ErrorCode = " + idwErrorCode.ToString());
                }

                return false;
            }
            catch (Exception e)
            {
                FileHelper.writeToLog(e.Message);
            }

            return false;

        }

    }
}
