using Attendance001.objects;
using System;
using System.Data.OracleClient;
using System.Diagnostics;

namespace Attendance001.helper
{
    class OrcHelper
    {
        private NetworkDetails netDet;
        private FileHelper fHelper = new FileHelper();

        private static OracleConnection orcConn;

        public OrcHelper()
        {
            netDet = fHelper.readNetworkDetails();
            if (orcConn == null)
                createConnection();
        }

        private void createConnection()
        {
            try
            {
                string connectionString =
            "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
            "(HOST=" + netDet.IP_DB + ")(PORT=" + netDet.PORT_DB + ")))" +
            "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + netDet.SID + ")));" +
            "User Id=" + netDet.DBNAME + ";Password= " + netDet.DBPASS + ";";
                //             string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + netDet.IP_DB + ")(PORT=" + netDet.PORT_DB + "))" +
                //"(CONNECT_DATA=(SID=" + netDet.SID + ")));User Id=" + netDet.DBNAME + ";Password=" + netDet.DBPASS + ";";

                orcConn = new OracleConnection(connectionString);
                orcConn.Open();

                NotificationHelper.CreateNotification("Connection Opened");
                Debug.Write("\r\n" + "Connection Opened");
            }
            catch (Exception e)
            {
                NotificationHelper.CreateNotification(e.Message);
                Debug.Write(e.Message);
            }
        }

        public void addRecordToTable(Record record)
        {
            try
            {
                //-----------Insert Query-----------------
                string query = "INSERT INTO EMPLOYEE_TIMINGS_AUTO(XCODE, YCODE, MCODE, HCODE, EDATE, TIME1) " +
                    " values (" + netDet.XCODE + "," + netDet.YCODE + "," + netDet.MCODE + "," + record.EnrollNumber + ",to_date('" + record.Date + "','mm/dd/yyyy'),'" + record.Time + "')";

                OracleCommand oram3 = new OracleCommand(query, orcConn);
                oram3.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.Write("\r\n" + e.Message);
            }
        }
    }
}



//------------Select Query---------------
//string com2 = "select * from " + dataArray[8] + " where COUNT=" + a + " and ACODE=" + b + " and EDATE=to_date('" + c + "','dd/mm/yyyy') and TIMIN='" + d + "' ";
//Debug.Write("\r\n" + com2);
//OracleDataAdapter oraADAPT2 = new OracleDataAdapter(com2, oraConn);
//OracleDataReader dr2;
//OracleCommand orcom2 = new OracleCommand(com2, oraConn);
//dr2 = orcom2.ExecuteReader();
//dr2.Read();
//string acode = dr2["ACODE"].ToString();
