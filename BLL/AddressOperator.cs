using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoCoding.Interface;
using GeoCoding.BaiDu;
using GeoCoding;
using System.IO;
using DBA;
namespace BLL
{
    public class AddressOperator
    {
        private static IGeoCoding geo = GeoCodingFactory.GetGeoCoding("baidu");
        private static log4net.ILog log = log4net.LogManager.GetLogger("AddressOperator");
        public static void Parse()
        {
            int maxid = SQLiteDbAccess.ExecuteScalarInt("select max(sn) from address");
            Console.WriteLine(maxid);
            string[] files = Directory.GetFiles(@"\\192.168.70.10\f$\CN_Process\target", "CN_Index_INDI.txt", SearchOption.AllDirectories);
            int j = 0;
            int tmpi = 0;
            foreach (var x in files)
            {
                tmpi++;
                Console.WriteLine(x);
                using (StreamReader sr = new StreamReader(x, Encoding.GetEncoding("gb2312")))
                {
                    while (!sr.EndOfStream)
                    {
                        j++;
                        string rl = sr.ReadLine().Trim();
                        if (string.IsNullOrEmpty(rl)) continue;
                        if (j % 100 == 0)
                        {
                            Console.WriteLine(j);
                        }
                        string[] aryrl = rl.Split('|');
                        string sn = aryrl[0];
                        string an = aryrl[1];
                        string city = aryrl[14];
                        string address = aryrl[15];
                        int cityno = 0;
                        int isn = 0;
                        if (!int.TryParse(sn, out isn))
                        {
                            continue;
                        }
                        if (maxid >= isn) continue;

                        string sql = sql = string.Format(@"
                                        insert into 
                                         address 
                                            (
                                            sn,
                                            shenqinghao,
                                            dizhi,
                                            guojia                                            
                                            ) 
                                        values 
                                            (
                                            {0},
                                            '{1}',
                                            '{2}',
                                            '{3}'
                                            )", sn, an, address, city);

                        //如果不是中国地址
                        if (!int.TryParse(city, out cityno))
                        {
                            Console.Write(city);
                            //直接插入SQL 
                            SQLiteDbAccess.ExecNoQuery(sql);
                            continue;
                        }                      



                        try
                        {
                            Location location = geo.GetLocation(address);
                            if (location != null)
                            {

                                AddressComponent AC = geo.GetAddressComponent(location);
                                if (AC != null)
                                {
                                    sql = string.Format(@"
                                        insert into 
                                         address 
                                            (
                                            sn,
                                            shenqinghao,
                                            dizhi,
                                            guojia,
                                            sheng,
                                            shi,
                                            quxian,
                                            street,
                                            lng,
                                            lat
                                            ) 
                                        values 
                                            (
                                            {0},
                                            '{1}',
                                            '{2}',
                                            '{3}',
                                            '{4}',
                                            '{5}',
                                            '{6}',
                                            '{7}',
                                            '{8}',
                                            '{9}' 
                                            )", sn, an, address, AC.Country, AC.Province, AC.City, AC.District, AC.Street, location.Lng, location.Lat);
                                }
                            }
                            SQLiteDbAccess.ExecNoQuery(sql);

                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.ToString());
                        }

                    }

                }
            }
            Console.WriteLine(string.Format("{0}/{1}处理完毕", tmpi, files.Length));
            //写 转换后的地址信息
        }

        public static void test()
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
                string an = "123123";
                int sn = 10;
                string sql = string.Format(@"
                                            insert into 
                                             address 
                                                (
                                                sn,
                                                shenqinghao,
                                                dizhi,
                                                guojia,
                                                sheng,
                                                shi,
                                                quxian,
                                                street,
                                                lng,
                                                lat
                                                ) 
                                            values 
                                                (
                                                {0},
                                                '{1}',
                                                '{2}',
                                                '{3}',
                                                '{4}',
                                                '{5}',
                                                '{6}',
                                                '{7}',
                                                '{8}',
                                                '{9}' 
                                                )", sn, an, address, AC.Country, AC.Province, AC.City, AC.District, AC.Street, location.Lng, location.Lat);
                SQLiteDbAccess.ExecNoQuery(sql);

            }
        }

    }
}
