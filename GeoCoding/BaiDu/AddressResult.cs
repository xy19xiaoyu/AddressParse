using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoCoding.BaiDu
{
    public class AddressResult
    {
        private int _Status;

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
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
        private AddressComponent _AddressComponent;

        public AddressComponent AddressComponent
        {
            get { return _AddressComponent; }
            set { _AddressComponent = value; }
        }
    }
}
