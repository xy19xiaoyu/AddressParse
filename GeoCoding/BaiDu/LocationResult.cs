using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoCoding.BaiDu
{
    public class LocationResult
    {
        private int _Status;

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private result _Reuslt;

        public result Reuslt
        {
            get { return _Reuslt; }
            set { _Reuslt = value; }
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
