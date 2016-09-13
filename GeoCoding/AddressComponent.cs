using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoCoding
{
    public class AddressComponent
    {
        private string city;

        /// <summary>
        /// 城市
        /// </summary>
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        private string country;

        /// <summary>
        /// 国家
        /// </summary>
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        private string district;

        /// <summary>
        /// 区域
        /// </summary>
        public string District
        {
            get { return district; }
            set { district = value; }
        }
        private string province;

        //省
        public string Province
        {
            get { return province; }
            set { province = value; }
        }
        private string street;

        //街道
        public string Street
        {
            get { return street; }
            set { street = value; }
        }
        private string street_number;

        //街道编码
        public string Street_number
        {
            get { return street_number; }
            set { street_number = value; }
        }
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4}", Country, Province, City, District, Street);
        }

        
    }
}
