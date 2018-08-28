using Attendance001.objects;
using System;
using System.Collections.Generic;
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
        private CZKEMClass axCZKEM1 = new CZKEMClass();

        ZKHelper()
        {
            NetworkDetails netDet = fHelper.readNetworkDetails();
        }

        private bool open()
        {
            
            return false;
        }

    }
}
