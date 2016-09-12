using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoCoding
{
    public class Location
    {
        private double lng;

        public double Lng
        {
            get { return lng; }
            set { lng = value; }
        }
        private double lat;

        public double Lat
        {
            get { return lat; }
            set { lat = value; }
        }
        public override string ToString()
        {
            return string.Format("{0},{1}", Lng, Lat);
        }
    }
}
