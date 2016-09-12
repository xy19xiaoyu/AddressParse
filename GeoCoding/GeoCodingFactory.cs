using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoCoding.Interface;
using GeoCoding.BaiDu;
namespace GeoCoding
{
    public class GeoCodingFactory
    {
        public static IGeoCoding GetGeoCoding(string name)
        {
            IGeoCoding tmp = null;
            switch (name)
            {
                case "baidu":
                    tmp = new BaiDuGeoCoding();
                    break;
                default:
                    tmp = new BaiDuGeoCoding();
                    break;
            }
            return tmp;
        }
    }
}
