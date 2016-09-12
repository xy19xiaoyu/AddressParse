using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoCoding.Interface;
using GeoCoding.BaiDu;
using GeoCoding;

namespace BLL
{
    public class AddressOperator
    {
        private static IGeoCoding geo = GeoCodingFactory.GetGeoCoding("baidu");
        public static void Parse()
        {
            //get address list
            Location location = geo.GetLocation("河南省安阳市滑县白道口镇前安村168号");
            if (location == null) return;
            AddressComponent AC = geo.GetAddressComponent(location);
            if (AC == null) return;
            Console.WriteLine(AC.ToString());
            //写 转换后的地址信息
        }

    }
}
