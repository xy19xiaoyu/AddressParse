using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoCoding.Interface;
using System.Net;
using System.IO;
using Newtonsoft;
using log4net;

namespace GeoCoding.BaiDu
{
    public class BaiDuGeoCoding : IGeoCoding
    {
        private static ILog log = LogManager.GetLogger("BaiDuGeoCoding");
        private static string _Url = "http://api.map.baidu.com/geocoder/v2/";
        private static string _OutPut = "json";
        private static string _AK = "rYOka8cdCaads2RxGtK3ICfyDmkNNBaa";
        private static string _CallBack = "";
        private static string _SK = "Ri5CxusEOLmKlIhgmqwAyTHDAfS2TNhm";

        private WebRequest _Request;
        #region  IGeoCoding 成员

        public string Url
        {
            get
            {
                return _Url;
            }
            set
            {
                _Url = value;
            }
        }

        public string OutPut
        {
            get
            {
                return _OutPut;
            }
            set
            {
                _OutPut = value;
            }
        }

        public string AK
        {
            get
            {
                return _AK;
            }
            set
            {
                _AK = value;
            }
        }

        public string CallBack
        {
            get
            {
                return _CallBack;
            }
            set
            {
                _CallBack = value;
            }
        }
        public System.Net.WebRequest Request
        {
            get
            {
                return _Request;
            }
            set
            {
                _Request = value;
            }
        }

        public string SK
        {
            get
            {
                return _SK;
            }
            set
            {
                _SK = value;
            }
        }
        public Location GetLocation(string Address)
        {
            string geturl = string.Format("address={1}&output={2}&ak={3}", Url, System.Web.HttpUtility.UrlDecode(Address), OutPut, AK);
            Dictionary<string, string> query = new Dictionary<string, string>();
            query.Add("address", Address);
            query.Add("output", OutPut);
            query.Add("ak", AK);

            //var queryString = HttpBuildQuery(query);
            //var str = UrlEncode(Url + "?" + queryString + SK);

            //query.Add("sn", MD5(str));
            string FullUrl = Url + "?" + HttpBuildQuery(query);
            string JSON = GetJSON(FullUrl);
            LocationResult result = Newtonsoft.Json.JsonConvert.DeserializeObject<LocationResult>(JSON);

            if (result.status == 0)
            {
                return result.Result.Location;
            }
            else
            {
                log.Error(FullUrl + Environment.NewLine + result.status + "\t");//+ result.message
                return null;
            }

        }

        public AddressComponent GetAddressComponent(Location location)
        {
            //ak=E4805d16520de693a3fe707cdc962045&callback=renderReverse&location=39.983424,116.322987&output=json&pois=1


            Dictionary<string, string> query = new Dictionary<string, string>();
            query.Add("location", location.ToString());
            query.Add("output", OutPut);
            query.Add("ak", AK);
            query.Add("callback", CallBack);
            query.Add("pois", "0");

            //var queryString = HttpBuildQuery(query);
            //var str = UrlEncode(Url + "?" + queryString + SK);

            //query.Add("sn", MD5(str));
            string FullUrl =Url + "?" + HttpBuildQuery(query);
            string JSON = GetJSON(FullUrl);
            AddressResult result = Newtonsoft.Json.JsonConvert.DeserializeObject<AddressResult>(JSON);

            if (result.Status == 0)
            {
                return result.Result.AddressComponent;
            }
            else
            {
                log.Error(FullUrl + Environment.NewLine + result.Status + "\t" + result.Message);
                return null;
            }

        }
        public string GetJSON(string Url)
        {
            string resultdata = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(Url);
                WebResponse response = request.GetResponse();
                Stream resStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(resStream, Encoding.UTF8);
                resultdata = sr.ReadToEnd();
                resStream.Close();
                sr.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return string.Empty;
            }

            return resultdata;
        }

        #endregion

        #region IGeoCoding 成员



        private static string MD5(string password)
        {
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(password);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    ret += a.ToString("x2");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        private static string UrlEncode(string str)
        {
            str = System.Web.HttpUtility.UrlEncode(str);
            byte[] buf = Encoding.ASCII.GetBytes(str);//等同于Encoding.ASCII.GetBytes(str)
            for (int i = 0; i < buf.Length; i++)
                if (buf[i] == '%')
                {
                    if (buf[i + 1] >= 'a') buf[i + 1] -= 32;
                    if (buf[i + 2] >= 'a') buf[i + 2] -= 32;
                    i += 2;
                }
            return Encoding.ASCII.GetString(buf);//同上，等同于Encoding.ASCII.GetString(buf)
        }
        private static string HttpBuildQuery(IDictionary<string, string> querystring_arrays)
        {

            StringBuilder sb = new StringBuilder();
            foreach (var item in querystring_arrays)
            {
                sb.Append(UrlEncode(item.Key));
                sb.Append("=");
                sb.Append(UrlEncode(item.Value));
                sb.Append("&");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        #endregion


    }
}
