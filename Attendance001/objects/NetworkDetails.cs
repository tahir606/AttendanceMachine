using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Attendance001.objects
{
    class NetworkDetails
    {
        //AM = Attendance Machine
        private int _XCODE /*Company Code*/, _YCODE /*Branch Code*/, _MCODE /*Machine Code*/;
        private string _IP_DB, _PORT_DB, _SID, _DBNAME, _DBPASS, _IP_AM, _PORT_AM;

        public override string ToString()
        {
            return base.ToString();
        }

        public string IP_DB { get => _IP_DB; set => _IP_DB = value; }
        public string PORT_DB { get => _PORT_DB; set => _PORT_DB = value; }
        public string SID { get => _SID; set => _SID = value; }
        public string DBNAME { get => _DBNAME; set => _DBNAME = value; }
        public string DBPASS { get => _DBPASS; set => _DBPASS = value; }
        public string IP_AM { get => _IP_AM; set => _IP_AM = value; }
        public string PORT_AM { get => _PORT_AM; set => _PORT_AM = value; }

        public int XCODE { get => _XCODE; set => _XCODE = value; }
        public int YCODE { get => _YCODE; set => _YCODE = value; }
        public int MCODE { get => _MCODE; set => _MCODE = value; }
    }
}
