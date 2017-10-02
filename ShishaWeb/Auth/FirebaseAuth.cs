using Newtonsoft.Json;
using ShishaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShishaWeb.Auth
{
    public static class FirebaseAuth
    {
        private static readonly string FirebaseApiKey = "AIzaSyClgHEjEdNlyEXcv_CBk3QDwXr31ZhNbpA";
        private static readonly string FirebaseRestApiUrl = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/";

        public static async Task<FirebaseContactInfo> GetAccountInfo(string firebaseToken)
        {
            var result = new FirebaseContactInfo();
            //todo
            //use string format like $"Got {posts.Count()} posts"
            var accountInfoUrl = FirebaseRestApiUrl + "getAccountInfo?key=" + FirebaseApiKey;
            var json = JsonConvert.SerializeObject(new
            {
                idToken = firebaseToken
            });

            var client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(accountInfoUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<FirebaseContactInfo>(jsonResult);
            }
            client.Dispose();

            return result;
        }
    }
}
