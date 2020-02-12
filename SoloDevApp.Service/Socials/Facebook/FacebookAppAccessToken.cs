using Newtonsoft.Json;

namespace SoloDevApp.Service.Socials.Facebook
{
    public class FacebookAppAccessToken
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}