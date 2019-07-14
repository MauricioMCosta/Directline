using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Directline.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Directline.Controllers
{
   
    [ApiController]
    public class ApiController : ControllerCommon
    {
        private ILogger<DirectlineController> _logger;

        public ApiController(
            IHttpContextAccessor httpContext,
            ILogger<DirectlineController> logger,
            IConfiguration config,
            IDataStorage storage) : base(httpContext, config, storage)
        {
            _logger = logger;
        }
        [HttpPost]
        [Route("/api/secret")]
        public ActionResult Secret([FromBody] BotData botData)
        {
            Claim[] claims = new Claim[]
            {
                new Claim("bot.serviceUrl",botData.Url),
                new Claim("bot.username", botData.Username),
                new Claim("bot.password", botData.Password)
            };

            var secret = GenerateJSONWebToken(claims, DateTime.Now.AddYears(10));
            return Ok(secret);
        }

        [HttpGet]
        [Route("/api/statistics")]
        public ActionResult Statistics()
        {
            var activeConversations = _datastorage.Conversations.Count;
            var countOfActivities = 0;
            foreach(var c in _datastorage.Conversations)
            {
                countOfActivities+=c.Value.History.Count;
            }
            var activeSince = _datastorage.StartupDateTime;
            var uptime = DateTime.UtcNow.Subtract(activeSince).ToString();
            return Ok(new { activeSince=activeSince, upTime=uptime, activeConversations=activeConversations, countOfActivities=countOfActivities });
        }
    }
}