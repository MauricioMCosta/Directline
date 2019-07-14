using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Directline.Models
{
    public class UserModel
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
