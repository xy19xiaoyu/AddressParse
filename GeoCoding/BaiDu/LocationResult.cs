using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoCoding.BaiDu
{

    public class LocationResult
    {
        private int _Status;

        public int status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private string _Message;

        public string message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        private result _Result;

        public result Result
        {
            get { return _Result; }
            set { _Result = value; }
        }

    }
    public class result
    {
        private Location _Location;

        public Location Location
        {
            get { return _Location; }
            set { _Location = value; }
        }
        public string precise;
        public string confidence;
        public string level;
    }
}
