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
            string[] addresses = {
                                     "北京豪威大厦", 
                                     "北京市海淀区大柳树路17号福海大厦", 
                                     "北京市海淀区花园东路乙9号院7单元1号楼601", 
                                     "河南省安阳市滑县白道口镇前安村",
                                     "河南省安阳市滑县西河京",
                                     "北京市朝阳区酒仙桥北路7号电通创意广场10号楼"
                                 };
            foreach (var address in addresses)
            {

                Console.WriteLine(address);
                //get address list
                Location location = geo.GetLocation(address);
                if (location == null) return;
                AddressComponent AC = geo.GetAddressComponent(location);
                if (AC == null) return;
                Console.WriteLine(AC.ToString() + "\n");

            }
            //写 转换后的地址信息
        }

    }
}
