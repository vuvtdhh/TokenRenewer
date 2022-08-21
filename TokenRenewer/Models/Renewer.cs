using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TokenRenewer.Helpers;

namespace TokenRenewer.Models
{
    class Renewer
    {
        public static string Work(TokenRenewerConfig config)
        {
            TokenRequestInfo TokenRequestInfo = SqlFunctions.GetTokenRequestInfo();

            TokenRequest tokenRequest = new TokenRequest();
            tokenRequest.grant_type = TokenRequestInfo.grant_type;
            tokenRequest.client_id = TokenRequestInfo.client_id;
            tokenRequest.client_secret = TokenRequestInfo.client_secret;
            tokenRequest.scope = TokenRequestInfo.scope;
            tokenRequest.session_id = TokenRequestInfo.session_id;

            string json = JsonConvert.SerializeObject(tokenRequest);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.PostAsync(TokenRequestInfo.request_url_0, content).Result;

            if (response.IsSuccessStatusCode)
            {
                // parse to Token.
                string result = response.Content.ReadAsStringAsync().Result;
                TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenInfo>(result);
                TokenInfo token = new TokenInfo();
                token.access_token = tokenResponse.access_token;
                token.expires_in = tokenResponse.expires_in;
                token.token_type = tokenResponse.token_type;
                token.scope = tokenResponse.scope;
                token.last_update_time = DateTime.Now;
                token.last_update_status = response.StatusCode.ToString();


                // update to DB.
                int updated = SqlFunctions.TokenInfoUpdate(token);

                return string.Format(string.Concat("\n", config.RenewSuccessMessage), DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), updated);
            }
            else
            {
                return string.Format(string.Concat("\n", config.RenewFailedMessage), DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), response.ToString());
            }
        }
    }
}
