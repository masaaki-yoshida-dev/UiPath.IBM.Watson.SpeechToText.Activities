using myoshidan.IBM.Watson.STT.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace myoshidan.IBM.Watson.STT.Models
{
    /// <summary>
    /// IBMWatsonGetAccessToken
    /// </summary>
    public class IBMWatsonGetAccessToken
    {
        private static HttpClient _client = new HttpClient();
        private static string _authUrl = "https://iam.cloud.ibm.com/identity/token";

        /// <summary>
        /// GetAccessToken
        /// </summary>
        /// <param name="apikey"></param>
        /// <returns></returns>
        public static async Task<string> GetAccessToken(string apikey)
        {
            var parameters = new Dictionary<string, string>()
            {
                { "grant_type", "urn:ibm:params:oauth:grant-type:apikey" },
                { "apikey", apikey },
            };
            var content = new FormUrlEncodedContent(parameters);

            var response = await _client.PostAsync(_authUrl, content);
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            return JsonConvert.DeserializeObject<AccessToken>(await response.Content.ReadAsStringAsync()).access_token;
        }
    }
}
