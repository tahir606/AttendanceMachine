using Attendance001.helper;
using Attendance001.objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Attendance001
{
    public partial class SettingsForm : Form
    {
        private FileHelper fHelper;

        public SettingsForm()
        {
            InitializeComponent();
            label11.Visible = false;

            fHelper = new FileHelper();
            NetworkDetails netDet = fHelper.readNetworkDetails();
            text_db_ip.Text = netDet.IP_DB;
            text_db_port.Text = netDet.PORT_DB;
            text_sid.Text = netDet.SID;
            text_db_name.Text = netDet.DBNAME;
            text_db_pass.Text = netDet.DBPASS;
            text_m_ip.Text = netDet.IP_AM;
            text_m_port.Text = netDet.PORT_AM;
            text_comp_code.Text = netDet.XCODE.ToString();
            text_branch_code.Text = netDet.YCODE.ToString();
            text_machine_code.Text = netDet.MCODE.ToString();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            string db_ip = text_db_ip.Text,
                db_port = text_db_port.Text,
                db_sid = text_sid.Text,
                db_name = text_db_name.Text,
                db_pass = text_db_pass.Text,
                m_ip = text_m_ip.Text,
                m_port = text_m_port.Text,
                comp_code = text_comp_code.Text,
                branch_code = text_branch_code.Text,
                machine_code = text_machine_code.Text;

            if (db_ip.Equals("") || db_port.Equals("") || db_sid.Equals("") || db_name.Equals("") || db_pass.Equals("") ||
                m_ip.Equals("") || m_port.Equals("") || comp_code.Equals("") || branch_code.Equals("") || machine_code.Equals(""))
            {
                label11.Visible = true;
            }
            else
            {
                NetworkDetails nDet = new NetworkDetails();
                nDet.IP_DB = db_ip;
                nDet.PORT_DB = db_port;
                nDet.SID = db_sid;
                nDet.DBNAME = db_name;
                nDet.DBPASS = db_pass;
                nDet.IP_AM = m_ip;
                nDet.PORT_AM = m_port;
                nDet.XCODE = int.Parse(comp_code);
                nDet.YCODE = int.Parse(branch_code);
                nDet.MCODE = int.Parse(machine_code);

                fHelper.WriteNetworkDetails(nDet);
                label11.Visible = false;

                this.Close();
            }
        }
    }
}
