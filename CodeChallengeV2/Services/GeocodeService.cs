using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallengeV2.Models;
using Newtonsoft.Json.Linq;

namespace CodeChallengeV2.Services
{
    public class GeocodeService : IGeocodeService
    {
        public Task<string> FindCountryCode(GeocodePayload payload)
        {
            string url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={payload.Lat}&lon={payload.Long}&zoom=0";

            var res = JObject.Parse(GetWebData(url));
            string countryCode = (string) res["address"]["country_code"];

            return Task.FromResult(countryCode.ToUpper());
        }

        private string GetWebData(string url) {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            StreamReader reader =  new StreamReader (response.GetResponseStream());
            
            string output = reader.ReadToEnd();

            reader.Close();
            response.Close();

            return output;
        }
    }
}
