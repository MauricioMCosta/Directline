using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Directline.Models;
using Directline.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Directline.Controllers
{

    [ApiController]
    public class DirectlineController : ControllerCommon
    {
        private readonly ILogger<DirectlineController> _logger;

        public DirectlineController(
            IHttpContextAccessor httpContext,
            ILogger<DirectlineController> logger,
            IConfiguration config,
            IDataStorage storage) : base(httpContext, config, storage)
        {
            _logger = logger;
        }

        #region Options
        [HttpOptions]
        [Route("/directline")]
        public ActionResult OptionDirectline()
        {
            _logger.LogDebug("OPTIONS /directline");
            return Ok();
        }


        #endregion

        [HttpPost]
        [Route("/directline/conversations")]
        [Route("/v3/directline/conversations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult CreateConversation()
        {
            _logger.LogDebug("POST /directline/conversations");

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity?.Claims;

            var conversation = new Conversation() {
                ConversationId = Guid.NewGuid().ToString(),
                BotEndpoint=new BotData()
                {
                    Url=claims?.FirstOrDefault(w=>w.Type.Equals("bot.serviceUrl"))?.Value,
                    Password= claims?.FirstOrDefault(w => w.Type.Equals("bot.password"))?.Value,
                    Username=claims?.FirstOrDefault(w => w.Type.Equals("bot.username"))?.Value
                }
            };

            _datastorage.Conversations.Add(conversation.ConversationId, conversation);

            var c = new ConversationObject() { ConversationId = conversation.ConversationId, ExpiresIn = expiresIn };
            return StatusCode((int)HttpStatusCode.Created, c);

        }

        // Sends message to bot client. Assumes message activities
        [HttpGet]
        [Route("/directline/conversations/{conversationId}/activities")]
        [Route("/v3/directline/conversations/{conversationId}/activities")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public ActionResult GetActivity([FromRoute] string conversationId, [FromQuery(Name = "watermark")] string w)
        {
            _logger.LogDebug("GET /directline/conversations/:conversationId/activities");

            var watermark = string.IsNullOrEmpty(w) ? 0 : Convert.ToInt32(w);
            Conversation conversation;

            if (_datastorage.Conversations.TryGetValue(conversationId, out conversation))
            {
                if (conversation.History.Count > watermark)
                {

                    var activities = conversation.History.Skip(watermark).ToArray();
                    return Ok(new { activities = activities, watermark = watermark + activities.Length });
                }
                else
                {
                    return Ok(new { activities = new List<Activity>(), watermark = watermark });
                }
            }
            return BadRequest(new { error = 400, message = "conversation error" });

        }
        // Sends message to bot. Assumes message activities
        [HttpPost]
        [Route("/directline/conversations/{conversationId}/activities")]
        [Route("/v3/directline/conversations/{conversationId}/activities")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PostActivityAsync([FromRoute] string conversationId)
        {
            _logger.LogDebug("POST /directline/conversations/:conversationId/activities");

            Activity incomingActivity;
            using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();
                incomingActivity = JsonConvert.DeserializeObject<Activity>(body);
            }

            var activity = new MessageActivity()
            {
                ServiceUrl = GetServiceUrl(),
                ChannelId = "emulator",
                Conversation = new ConversationAccount() { Id = conversationId }
            };
            activity.CopyFrom(incomingActivity, new string[] { "Id", "Timestamp", "LocalTimestamp", "ServiceUrl", "Conversation" });

            var conversation = GetConversation(conversationId);
             
            if (conversation?.BotEndpoint?.Url != null)
            {
                conversation.History.Add(activity);
                // --> Send to BOT!
                var statusCode = 200;
                try
                {
                    using (var client = new HttpClient())
                    {
                        //client.BaseAddress = new Uri(conversation.BotEndpoint.Url);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var response = await client.PostAsJsonAsync(conversation.BotEndpoint.Url, activity);
                        var c = new ConversationObject() { ConversationId = conversation.ConversationId, ExpiresIn = expiresIn };
                        return StatusCode((int)response.StatusCode, c);
                    }
                }
                catch (WebException e)
                {
                    _logger.LogError($"Can't connect to BOT {conversation.BotEndpoint.Url}", e.StackTrace);
                    statusCode = 400;
                    return StatusCode(statusCode, new { error = 400, message = $"can't connect to bot {conversation.BotEndpoint.Url}" });
                }

            }
            // conversation never was initiated
            _logger.LogError("Conversation or BOT endpoint is null... Initialize or check endpoints in token");
            return BadRequest();
        }

        [HttpGet]
        [Route("/directline/conversations/{conversationId}")]
        [Route("/v3/directline/conversations/{conversationId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public void GetConversation([FromRoute] string conversationId, [FromQuery(Name = "watermark")] string w)
        {
            return;
            ////return BadRequest(new { error = 400, message = "unsupported" });
            //_logger.LogDebug($"GET /directline/conversations/{conversationId}");
            //_logger.LogInformation("Reestablishing conversation");
            //ConversationObject obj;
            //Conversation conversation;
            //if (_datastorage.Conversations.TryGetValue(conversationId, out conversation))
            //{
            //    return Ok(new ConversationObject() { ConversationId = conversationId, ExpiresIn = expiresIn });
            //}
            //return BadRequest();
        }

        [HttpPost]
        [Route("/directline/tokens/generate")]
        [Route("/v3/directline/tokens/generate")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult GenerateToken([FromBody]UserModel user)
        {
            // the magic here is to get the claims from the initial Authentication token
            // based on that just generate a temporary token with proper expiration

            // NOT FINISHED! Just keeping this code as future reference.

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                //    identity.FindFirst("ClaimName").Value;

            }
            //GenerateJSONWebToken()
            return Ok();
        }
    }
}