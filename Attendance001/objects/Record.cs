using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Attendance001.objects
{
    class Record
    {
        private int _enrollNumber;
        private string _date, _time;

        public Record(int enrollNumber, string date, string time)
        {
            _enrollNumber = enrollNumber;
            _date = date;
            _time = time;
        }

        public int EnrollNumber { get => _enrollNumber; set => _enrollNumber = value; }
        public string Date { get => _date; set => _date = value; }
        public string Time { get => _time; set => _time = value; }
    }
}
