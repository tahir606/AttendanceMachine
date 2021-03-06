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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;
using zkemkeeper;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace Attendance001
{
    public partial class Form1 : Form
    {
        private ZKHelper zkHelper;
        private OrcHelper orcHelper;

        public Form1()
        {
            InitializeComponent();          

            zkHelper = new ZKHelper();
            orcHelper = new OrcHelper();
            

            FileHelper.writeToLog("Init");
            dispose();
        }

        #region Communication
        private int iMachineNumber = 1;//the serial number of the device.After connecting the device ,this value will be changed.

        //If your device supports the TCP/IP communications, you can refer to this.
        //when you are using the tcp/ip communication,you can distinguish different devices by their IP address.
        private void dispose()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    loadAndInsert();

                    Thread.Sleep(5 * 60 * 1000); //5 Min
                }
            }
            ).Start();


        }

        private void form1_formClosing(object sender, FormClosingEventArgs e)
        {
            FileHelper.writeToLog("Closing");
            ZKHelper.axCZKEM1.Disconnect();
        }
        #endregion


        private void loadAndInsert()
        {
            NetworkDetails netDet = new FileHelper().readNetworkDetails();

            int iglcount = 0, iindex = 0;
            FileHelper.writeToLog("Getting Records");
            List<Record> records = zkHelper.getAllLogData();
            FileHelper.writeToLog("Got Records");
            if (records == null)
            {
                FileHelper.writeToLog("\r\n Null records");
                NotificationHelper.CreateNotification("Null records");
                return;
            }

            //string fDate = DateTime.Now.ToString("M/d/yyyy");        
            //string fDate = "8/30/2018";
            string fDate = orcHelper.getLastDateAdded();                   
            DateTime maxEdate = DateTime.Parse(fDate);

            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate
                {

                    lvLogs.Items.Clear();

                    foreach (Record record in records)
                    {
                        DateTime rDate = DateTime.Parse(record.Date);
                        if (rDate >= maxEdate)
                        {
                            iglcount++;
                            lvLogs.Items.Add(iglcount.ToString());
                            lvLogs.Items[iindex].SubItems.Add(record.EnrollNumber.ToString());
                            lvLogs.Items[iindex].SubItems.Add(record.Date.ToString());
                            lvLogs.Items[iindex].SubItems.Add(record.Time.ToString());
                            iindex++;
                        }
                    }

                    foreach (Record record in records)
                    {
                        DateTime rDate = DateTime.Parse(record.Date);
                        if (rDate >= maxEdate)
                        {
                            orcHelper.addRecordToTable(record);
                        }
                    }
                }));
            }

        }        

        private void Btn_Settings_Click(object sender, EventArgs e)
        {
            SettingsForm sForm = new SettingsForm();
            sForm.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void helloWorldLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
