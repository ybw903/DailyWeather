using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using Windows.Data.Json;
using System.Runtime.Serialization;
using Windows.Storage;

namespace UWPWeather
{
    public class OpenWeatherMapProxy
    {
        public async static Task<RootObject> GetWeather(int lat, int lon)
        {
            var http = new HttpClient();
            string date = DateTime.Now.ToString("yyyyMMdd");

            StorageFile file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"api_key.txt");
            string key = await FileIO.ReadTextAsync(file);
            string url = "http://apis.data.go.kr/1360000/VilageFcstInfoService/getVilageFcst"; // URL
            url += "?ServiceKey=" + key; // Service Key
            url += "&pageNo=1";
            url += "&numOfRows=80";
            url += "&dataType=JSON";
            url += "&base_date="+date;
            url += "&base_time=0500";
            url += "&nx="+lat.ToString();
            url += "&ny="+lon.ToString();
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            
            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));

            var data = (RootObject)serializer.ReadObject(ms);
            return data;
        }
        [DataContract]
        public class Header
        {
            [DataMember]
            public string resultCode { get; set; }
            [DataMember]
            public string resultMsg { get; set; }
        }
        [DataContract]
        public class Item
        {
            [DataMember]
            public string baseDate { get; set; }
            [DataMember]
            public string baseTime { get; set; }
            [DataMember]
            public string category { get; set; }
            [DataMember]
            public string fcstDate { get; set; }
            [DataMember]
            public string fcstTime { get; set; }
            [DataMember]
            public string fcstValue { get; set; }
            [DataMember]
            public int nx { get; set; }
            [DataMember]
            public int ny { get; set; }
        }
        [DataContract]
        public class Items
        {
            [DataMember]
            public List<Item> item { get; set; }
        }
        [DataContract]
        public class Body
        {
            [DataMember]
            public string dataType { get; set; }
            [DataMember]
            public Items items { get; set; }
            [DataMember]
            public int pageNo { get; set; }
            [DataMember]
            public int numOfRows { get; set; }
            [DataMember]
            public int totalCount { get; set; }
        }
        [DataContract]
        public class Response
        {
            [DataMember]
            public Header header { get; set; }
            [DataMember]
            public Body body { get; set; }
        }
        [DataContract]
        public class RootObject
        {
            [DataMember]
            public Response response { get; set; }
        }
    }
}