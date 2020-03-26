using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace React.Sample.Webpack.CoreMvc
{
    public class AccessGraph
    {
        public string GetToken(string clientid, string secret, string redirect, string refreshtoken, string authCode, string tenant, string resource = "user.read")
        {
            string result = string.Empty;
            MSGraphAuthTokens tokens = null;
            using (var webClient = new WebClient())
            {
                var url = $"https://login.microsoftonline.com/" + tenant + "/oauth2/v2.0/token";// AAD
                url = $"https://login.microsoftonline.com/common/oauth2/token";// live API
                using (var httpClient = new HttpClient())
                {
                    Uri requestURL = new Uri(url);
                    var properties = "client_id=" + clientid + "&client_secret=" + secret + "&scope=" + resource + "&redirect_uri=" + redirect;
                    if (!string.IsNullOrEmpty(refreshtoken))
                    {
                        properties += "&refresh_token=" + refreshtoken + "&grant_type=refresh_token";
                    }
                    if (!string.IsNullOrEmpty(authCode))
                    {
                        properties += "&code=" + authCode + "&grant_type=authorization_code";
                    }
                    var content = new StringContent(properties, Encoding.UTF8, "application/x-www-form-urlencoded");
                    var res = httpClient.PostAsync(url, content).Result;
                    string resultJson = res.Content.ReadAsStringAsync().Result;
                    if (res.IsSuccessStatusCode)
                    {
                        tokens = JsonConvert.DeserializeObject<MSGraphAuthTokens>(resultJson);
                        result = tokens.access_token;
                    }
                }
            }
            return result;
        }
    }
    public class MSGraphAuthTokens
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
        public string id_token { get; set; }
    }
}
