using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoCoding
{
    public class AddressComponent
    {
        private string city;

        public string City
        {
            get { return city; }
            set { city = value; }
        }
        private string country;

        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        private string district;

        public string District
        {
            get { return district; }
            set { district = value; }
        }
        private string province;

        public string Province
        {
            get { return province; }
            set { province = value; }
        }
        private string street;

        public string Street
        {
            get { return street; }
            set { street = value; }
        }
        private string street_number;

        public string Street_number
        {
            get { return street_number; }
            set { street_number = value; }
        }
    }
}
