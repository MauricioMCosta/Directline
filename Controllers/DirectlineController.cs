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
        public ActionResult CreateConversation([FromHeader] string authorization)
        {
            _logger.LogDebug("POST /directline/conversations");
            _logger.LogInformation($"Authorization secret={authorization}");

            var conversation = new Conversation() { ConversationId = Guid.NewGuid().ToString() };

            _datastorage.Conversations.Add(conversation.ConversationId, conversation);

            var c = new ConversationObject() { ConversationId = conversation.ConversationId, ExpiresIn = expiresIn };
            return StatusCode((int)HttpStatusCode.Created, c);

        }

        // Sends message to bot client. Assumes message activities
        [HttpGet]
        [Route("/directline/conversations/{conversationId}/activities")]
        [Route("/v3/directline/conversations/{conversationId}/activities")]
        public ActionResult GetActivity([FromRoute] string conversationId, [FromQuery(Name = "watermark")] string w)
        {
            _logger.LogDebug("GET /directline/conversations/:conversationId/activities");
            _logger.LogInformation("Retrieving a message activity");
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
        public async Task<ActionResult> PostActivityAsync([FromRoute] string conversationId)
        {
            _logger.LogDebug("POST /directline/conversations/:conversationId/activities");
            _logger.LogInformation("Creating a message activity");
            Activity incomingActivity;
            using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();
                incomingActivity = JsonConvert.DeserializeObject<Activity>(body);
            }
                
            var activity = new MessageActivity()
            {
                ServiceUrl = GetServiceUrl(),
                ChannelId="emulator",
                Conversation = new ConversationAccount() { Id = conversationId }
            };
            activity.CopyFrom(incomingActivity, new string[] { "Id", "Timestamp", "LocalTimestamp", "ServiceUrl", "Conversation" });

            var conversation = GetConversation(conversationId);
            if (conversation != null)
            {
                conversation.History.Add(activity);
                // --> Send to BOT!
                var statusCode = 200;
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(GetBotUrl());
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var response = await client.PostAsJsonAsync($"api/messages", activity);
                        var c = new ConversationObject() { ConversationId = conversation.ConversationId, ExpiresIn = expiresIn };
                        return StatusCode((int)response.StatusCode, c);
                    }
                }
                catch (WebException e)
                {
                    _logger.LogError($"Can't connect to BOT {GetBotUrl()}", e.StackTrace);
                    statusCode = 400;
                    return StatusCode(statusCode, new { error = 400, message = "can't connect to bot" });
                }

            }
            // conversation never was initiated
            _logger.LogError("Conversation was not initiated");
            return BadRequest();
        }


        [HttpGet]
        [Route("/directline/conversations/{conversationId}")]
        [Route("/v3/directline/conversations/{conversationId}")]
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
        public ActionResult GenerateToken([FromBody]UserModel user)
        {

            return Ok();
        }
    }
}