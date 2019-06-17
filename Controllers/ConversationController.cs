using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Directline.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Directline.Controllers
{
    [ApiController]
    public class ConversationController : ControllerCommon
    {
        private readonly ILogger<ConversationController> _logger;
        public ConversationController(ILogger<ConversationController> logger, IConfiguration config, IDataStorage storage) : base(config, storage)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/v3/conversations/{conversationId}/members")]
        public ActionResult GetConversationMembers([FromRoute] string conversationId, [FromBody] string data)
        {
            _logger.LogDebug("GET /v3/conversations/:conversationId/members");
            return Ok();
        }
        
        [HttpGet]
        [Route("/v3/conversations/{conversationId}/activities/{activityId}/members")]
        public ActionResult GetActivityMembers([FromRoute] string conversationId, [FromRoute] string activityId, [FromBody] string data)
        {
            _logger.LogDebug("GET /v3/conversations/:conversationId/activities/:activityId/members");
            return Ok();
        }

        [HttpPost]
        [Route("/v3/conversations")]
        public ActionResult PostConversation([FromBody] string data)
        {
            _logger.LogDebug("POST /v3/conversations");
            _logger.LogWarning("POST /v3/conversations not implemented");
            return Ok();
        }

        [HttpPost]
        [Route("/v3/conversations/{conversationId}/activities")]
        public ActionResult PostActivity([FromRoute] string conversationId, [FromBody] Activity activity)
        {
            _logger.LogDebug("POST /v3/conversations/:conversationId/activities");
            

            activity.Id = Guid.NewGuid().ToString();
            activity.From = new ChannelAccount() { Id = "SkillBot", Name = "SkillBot" };

            var conversation = GetConversation(conversationId);
            if(conversation!=null)
            {
                conversation.History.Add(activity);
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("/v3/conversations/{conversationId}/activities/{activityId}")]
        public ActionResult PostConversationByActivityId([FromRoute] string conversationId, [FromRoute] string activityId)
        {
            _logger.LogDebug("POST /v3/conversations/:conversationId/activities/:activityId");
            _logger.LogInformation("Receiving activity response from BOT");

            Activity activity;
            using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();
                activity = JsonConvert.DeserializeObject<Activity>(body);
            }

            activity.Id = Guid.NewGuid().ToString();
            activity.From = new ChannelAccount() { Id = "SkillBot", Name = "SkillBot" };
            activity.ChannelData = new ChannelData() { ClientActivityID = activityId };

            var conversation = GetConversation(conversationId);
            if (conversation != null)
            {
                conversation.History.Add(activity);
                return Ok();
            }
            return BadRequest();
        }
    }
}