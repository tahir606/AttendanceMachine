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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void form1_formClosing(object sender, FormClosingEventArgs e)
        {
            Debug.Write("Closing");
            ZKHelper.axCZKEM1.Disconnect();
        }
        #endregion


        private void loadAndInsert()
        {
            NetworkDetails netDet = new FileHelper().readNetworkDetails();            

            int iglcount = 0, iindex = 0;

            List<Record> records = zkHelper.getAllLogData();
            if (records == null)
            {
                Debug.Write("\r\n Null records");
                return;
            }

            string fDate = DateTime.Now.ToString("M/d/yyyy");
            //string fDate = "8/30/2018";

            if (InvokeRequired)
            { 
                this.Invoke(new MethodInvoker(delegate {

                    lvLogs.Items.Clear();

                    foreach (Record record in records)
                    {
                        if (record.Date.Equals(fDate))
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
                        if (record.Date.Equals(fDate))
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
    }
}
