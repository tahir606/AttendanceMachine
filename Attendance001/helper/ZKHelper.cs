using Attendance001.objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using zkemkeeper;

namespace Attendance001.helper
{
    class ZKHelper
    {
        private bool bIsConnected = false;//the boolean value identifies whether the device is connected
        private int iMachineNumber = 1;//the serial number of the device.After connecting the device ,this value will be changed.

        //Create Standalone SDK class dynamicly.
        public static CZKEMClass axCZKEM1 = new CZKEMClass();
        private FileHelper fHelper = new FileHelper();

        private NetworkDetails netDet;

        public ZKHelper()
        {
            netDet = fHelper.readNetworkDetails();
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
                    Record record = new Record(int.Parse(sdwEnrollNumber),
                        idwMonth.ToString() + "/" + idwDay.ToString() + "/" + idwYear.ToString(),
                        idwHour.ToString() + ":" + idwMinute.ToString() + ":" + idwSecond.ToString());
                    records.Add(record);
                }

                return records;
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);

                if (idwErrorCode != 0)
                {
                    Debug.Write("Reading data from terminal failed,ErrorCode: " + idwErrorCode.ToString(), "Error");
                }
                else
                {
                    Debug.Write("No data from terminal returns!", "Error");
                }
            }

            //axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device               
            axCZKEM1.Disconnect();

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
                Debug.Write("Deleting records");
                //axCZKEM1.DeleteAttlogBetweenTheDate(iMachineNumber, "7/5/2012", "8/5/2012");
                //YYYY - MM - DD hh: mm: ss
                bool ret = axCZKEM1.DeleteAttlogBetweenTheDate(iMachineNumber, "2018-8-29 00:00:00", "2018-8-30 23:59:59");
                //bool ret = axCZKEM1.DeleteAttlogByTime(iMachineNumber, "2018-1-23 00:00:00");
                //bool ret = axCZKEM1.ClearGLog(iMachineNumber);

                if(!ret)
                {
                    int idwErrorCode = 0;
                    axCZKEM1.GetLastError(ref idwErrorCode);
                    Debug.Write("Operation failed, ErrorCode = " + idwErrorCode.ToString());
                }

                return false;
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }

            return false;

        }

    }
}
