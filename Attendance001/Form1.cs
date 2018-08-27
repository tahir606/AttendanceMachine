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
        //Create Standalone SDK class dynamicly.
        public CZKEMClass axCZKEM1 = new CZKEMClass();

        public Form1()
        {
            InitializeComponent();
            Debug.Write("\r\nInit");
            dispose();
        }

        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    //this.WindowState = FormWindowState.Minimized;
        //    Debug.Write("Starting to dispose");
        //    dispose();

        //}

        #region Communication
        private bool bIsConnected = false;//the boolean value identifies whether the device is connected
        private int iMachineNumber = 1;//the serial number of the device.After connecting the device ,this value will be changed.

        //If your device supports the TCP/IP communications, you can refer to this.
        //when you are using the tcp/ip communication,you can distinguish different devices by their IP address.
        private void dispose()
        {
            var textLines = File.ReadAllLines("orcl.txt");
            string[] dataArray = textLines;
            bIsConnected = axCZKEM1.Connect_Net(dataArray[6], Convert.ToInt32(dataArray[7]));
            if (bIsConnected == true)
            {
                // popupNotifier1.Scroll = true;
                //  popupNotifier1.ContentText = "device connected ";
                // popupNotifier1.Popup();

                lvLogs.Items.Clear();
                if (bIsConnected == false)
                {
                    MessageBox.Show("Please connect the device first", "Error");
                    return;
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

                int iGLCount = 0;
                int iIndex = 0;

                Cursor = Cursors.WaitCursor;
                lvLogs.Items.Clear();
                axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
                if (axCZKEM1.ReadGeneralLogData(iMachineNumber))//read all the attendance records to the memory
                {
                    while (axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode,
                                out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
                    {
                        iGLCount++;
                        lvLogs.Items.Add(iGLCount.ToString());
                        lvLogs.Items[iIndex].SubItems.Add(sdwEnrollNumber);//modify by Darcy on Nov.26 2009
                        lvLogs.Items[iIndex].SubItems.Add(idwMonth.ToString() + "/" + idwDay.ToString() + "/" + idwYear.ToString());
                        lvLogs.Items[iIndex].SubItems.Add(idwHour.ToString() + ":" + idwMinute.ToString() + ":" + idwSecond.ToString());
                        iIndex++;
                    }
                }
                else
                {
                    Cursor = Cursors.Default;
                    axCZKEM1.GetLastError(ref idwErrorCode);

                    if (idwErrorCode != 0)
                    {
                        MessageBox.Show("Reading data from terminal failed,ErrorCode: " + idwErrorCode.ToString(), "Error");
                    }
                    else
                    {
                        MessageBox.Show("No data from terminal returns!", "Error");
                    }
                }
                axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
                Cursor = Cursors.Default;
                axCZKEM1.Disconnect();

                string con2 = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + dataArray[0] + ")(PORT=" + dataArray[1] + "))" +
   "(CONNECT_DATA=(SID=" + dataArray[2] + ")));User Id=" + dataArray[3] + ";Password=" + dataArray[4] + ";";
                Debug.Write("\r\n" + con2);
                OracleConnection oraConn = new OracleConnection(con2);
                try
                {
                    oraConn.Open();
                    Debug.Write("\r\n" + "Connection Opened");
                    string tDate = DateTime.Now.ToString("dd/MM/yyyy");
                    //string fDate = DateTime.ParseExact(tDate, "dd/MM/yyyy",
                    //                   System.Globalization.CultureInfo.InvariantCulture).ToString().Split(' ')[0];
                    string fDate = "7/5/2012";
                    Debug.Write("\r\n" + fDate);
                    for (int i = 0; i < lvLogs.Items.Count; i++)
                    {
                        string a = lvLogs.Items[i].Text;
                        string b = lvLogs.Items[i].SubItems[1].Text;
                        string c = lvLogs.Items[i].SubItems[2].Text;
                        string d = lvLogs.Items[i].SubItems[3].Text;
                        string t = d.ToString();
                        string[] wordt = t.Split(':');

                        string h = "";
                        string m = "";
                        string ss = "";
                        string time = "";

                        if (wordt[0].Length == 1)
                        {
                            h = "0" + wordt[0];
                        }
                        else
                        {
                            h = wordt[0];
                        }

                        if (wordt[1].Length == 1)
                        {
                            m = "0" + wordt[1];
                        }
                        else
                        {
                            m = wordt[1];
                        }

                        if (wordt[2].Length == 1)
                        {
                            ss = "0" + wordt[2];
                        }
                        else
                        {
                            ss = wordt[2];
                        }
                        time = h + ":" + m + ":" + ss;

                        if (c == fDate)
                        {
                            //------------Select Query---------------
                            //string com2 = "select * from " + dataArray[8] + " where COUNT=" + a + " and ACODE=" + b + " and EDATE=to_date('" + c + "','dd/mm/yyyy') and TIMIN='" + d + "' ";
                            //Debug.Write("\r\n" + com2);
                            //OracleDataAdapter oraADAPT2 = new OracleDataAdapter(com2, oraConn);
                            //OracleDataReader dr2;
                            //OracleCommand orcom2 = new OracleCommand(com2, oraConn);
                            //dr2 = orcom2.ExecuteReader();
                            //dr2.Read();
                            //string acode = dr2["ACODE"].ToString();

                            try
                            {
                                //-----------Insert Query-----------------
                                //Debug.Write("ECODE: " + b + " DATE: " + c + " TIME: " + time);
                                string com3 = "insert into employee_timings(ECODE,EDATE,TIME1,REMKS) values (" + b + ",to_date('" + c + "','mm/dd/yyyy'),'" + time.Substring(0, 5) + "','HANDMACHINE')";
                                Debug.Write(com3);
                                OracleCommand oram3 = new OracleCommand(com3, oraConn);
                                oram3.ExecuteNonQuery();
                            }
                            catch (Exception e)
                            {
                                Debug.Write("\r\n" + e.Message);
                            }

                        }

                    }
                    dd();
                }
                catch (Exception e)
                {
                    Debug.Write(e.Message);
                    //Debug.Write("check your database connection");
                    //popupNotifier1.Scroll = true;
                    //popupnotifier1.contenttext = "check your database connection";
                    //popupnotifier1.popup();
                    //  Thread.Sleep(100000);
                    dd();
                }
            }
            else
            {
                Debug.Write("unable to connect the device");
                //popupNotifier1.Scroll = true;
                //popupNotifier1.ContentText = "unable to connect the device ";
                //popupNotifier1.Popup();
                //   Thread.Sleep(100000);
                dd();
            }
            // Thread.Sleep(100000);
            dd();
        }
        #endregion

        private void dd()
        {
            //this.progressBar1.Value = 0;
            //this.timer1.Interval = 10;
            //this.timer1.Enabled = true;
            //dispose2();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Click on the link below to continue learning how to build a desktop app using WinForms!
            System.Diagnostics.Process.Start("http://aka.ms/dotnet-get-started-desktop");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thanks!");
        }

        private void helloWorldLabel_Click(object sender, EventArgs e)
        {

        }

        private void lvLogs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
