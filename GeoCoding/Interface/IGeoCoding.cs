using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace GeoCoding.Interface
{
    public interface IGeoCoding
    {
        string Url { get; set; }
        string OutPut { get; set; }
        string AK { get; set; }
        string CallBack { get; set; }
        WebRequest Request { get; set; }

        Location GetLocation(string Address);

        AddressComponent GetAddressComponent(Location location);

        string GetJSON(string Url);

        string SK { get; set; }

    }
}
