using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Directline.Models
{
    /// <summary>
    /// Holds data for BOT registration (secret generation)
    /// This DirectLine implementation don't hold data. Just act as a bridge to multiple bots.
    /// </summary>
    public class BotData
    {
        /// <summary>
        /// BOT Service URL - it must include the /api/messages controller endpoint
        /// </summary>
        /// 
        [JsonProperty("url")]
        [Required]
        public string Url { get; set; }
        /// <summary>
        /// BOT Access Username - optional
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }
        /// <summary>
        /// BOT Access Password - optional 
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
