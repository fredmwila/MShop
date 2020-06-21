using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MShop
{
    public class CurrencyConvertor
    {
        public static Single GetExchangeRate(string toCurrency)
        {
            string exchangeRateURL = "http://www.floatrates.com/widget/00001049/4afbc5fe53d2436f0a84f70fea5aeb01/nzd.json";

            string getExchangeRate = Get(exchangeRateURL);

            dynamic currency = JsonConvert.DeserializeObject(getExchangeRate);

           double exchangeRate = currency[toCurrency.ToLower()]["rate"].Value;

            return Convert.ToSingle(exchangeRate);
        }

        public static string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
