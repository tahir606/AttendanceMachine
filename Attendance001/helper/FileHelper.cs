using Attendance001.objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Attendance001.helper
{
    class FileHelper
    {
        public NetworkDetails readNetworkDetails()
        {
            var textLines = File.ReadAllLines("orcl.txt");
            string[] dataArray = textLines;

            NetworkDetails networkDetails = new NetworkDetails();

            networkDetails.IP_DB = dataArray[0];
            networkDetails.PORT_DB = dataArray[1];
            networkDetails.SID = dataArray[2];
            networkDetails.DBNAME = dataArray[3];
            networkDetails.DBPASS = dataArray[4];
            networkDetails.IP_AM = dataArray[5];
            networkDetails.PORT_AM = dataArray[6];

            networkDetails.XCODE = int.Parse(dataArray[7]);
            networkDetails.YCODE = int.Parse(dataArray[8]);
            networkDetails.MCODE = int.Parse(dataArray[9]);

            return networkDetails;
        }

        public void WriteNetworkDetails(NetworkDetails nDet)
        {
            File.WriteAllText("orcl.txt", nDet.IP_DB + "\r\n" +
                nDet.PORT_DB + "\r\n" +
                nDet.SID + "\r\n" +
                nDet.DBNAME + "\r\n" +
                nDet.DBPASS + "\r\n" +
                nDet.IP_AM + "\r\n" +
                nDet.PORT_AM + "\r\n" +
                nDet.XCODE + "\r\n" +
                nDet.YCODE + "\r\n" +
                nDet.MCODE);
        }
    }

}
