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
        private string _Message;

        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        private ACresult _Result;

        public ACresult Result
        {
            get { return _Result; }
            set { _Result = value; }
        }

    }
    public class ACresult
    {
        private AddressComponent _AddressComponent;

        public AddressComponent AddressComponent
        {
            get { return _AddressComponent; }
            set { _AddressComponent = value; }
        }
    }
}
