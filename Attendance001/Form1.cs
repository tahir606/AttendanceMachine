using Attendance001.helper;
using Attendance001.objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zkemkeeper;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace Attendance001
{
    public partial class Form1 : Form
    {
        private ZKHelper zkHelper = new ZKHelper();
        private OrcHelper orcHelper = new OrcHelper();

        public Form1()
        {
            InitializeComponent();
            Debug.Write("\r\nInit");
            dispose();
        }

        #region Communication
        private int iMachineNumber = 1;//the serial number of the device.After connecting the device ,this value will be changed.

        //If your device supports the TCP/IP communications, you can refer to this.
        //when you are using the tcp/ip communication,you can distinguish different devices by their IP address.
        private void dispose()
        {
            NetworkDetails netDet = new FileHelper().readNetworkDetails();

            lvLogs.Items.Clear();

            int iglcount = 0, iindex = 0;

            Debug.Write("\r\n" + zkHelper.deleteLogData());

            List<Record> records = zkHelper.getAllLogData();
            if(records == null)
            {
                Debug.Write("\r\n Null records");
                return;
            }
            foreach (Record record in records)
            {
                iglcount++;
                lvLogs.Items.Add(iglcount.ToString());
                lvLogs.Items[iindex].SubItems.Add(record.EnrollNumber.ToString());
                lvLogs.Items[iindex].SubItems.Add(record.Date.ToString());
                lvLogs.Items[iindex].SubItems.Add(record.Time.ToString());
                iindex++;
            }

            string tDate = DateTime.Now.ToString("dd/MM/yyyy");
            string fDate = DateTime.ParseExact(tDate, "dd/MM/yyyy",
                               System.Globalization.CultureInfo.InvariantCulture).ToString().Split(' ')[0];

            //foreach (Record record in records)
            //{
            //    if (record.Date.Equals(fDate))
            //    {
            //        Debug.Write("Adding to Table");
            //        orcHelper.addRecordToTable(record);
            //    }
            //}

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void form1_formClosing(object sender, FormClosingEventArgs e)
        {
            Debug.Write("Closing");
            ZKHelper.axCZKEM1.Disconnect();
        }
        #endregion


        //void Form1Closed(object sender, FormClosedEventArgs e)
        //{
        //    // do something useful
        //    Debug.Write("Closing");
        //    ZKHelper.axCZKEM1.Disconnect();
        //}

    }
}
