using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Directline.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Directline.Controllers
{
    [ApiController]
    public class BotStateController : ControllerCommon
    {
        private readonly ILogger<BotStateController> _logger;

        public BotStateController(IHttpContextAccessor httpContext, 
            ILogger<BotStateController> 
            logger,IConfiguration config, 
            IDataStorage storage) : base(httpContext, config, storage)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/v3/botstate/{channelId}/users/{userId}")]
        public ActionResult GetUserState([FromRoute] string channelId, [FromRoute] string userId, [FromBody] BotState state)
        {
            _logger.LogDebug("GET /v3/botstate/:channelId/users/:userId");
            _logger.LogInformation("Getting User's bot state");
            var key = GetBotDataKey(channelId: channelId, userId: userId);
            return Ok(base.GetBotData(key));
        }


        [HttpGet]
        [Route("/v3/botstate/{channelId}/conversations/{conversationId}")]
        public ActionResult GetConversationState([FromRoute] string channelId, [FromRoute] string conversationId, [FromBody] BotState state)
        {
            _logger.LogDebug("GET /v3/botstate/:channelId/conversations/:conversationId");
            _logger.LogInformation("Getting Conversation's bot state");
            var key = GetBotDataKey(channelId: channelId, conversationId: conversationId);
            return Ok(base.GetBotData(key));
        }

        [HttpGet]
        [Route("/v3/botstate/{channelId}/conversations/{conversationId}/users/{userId}")]
        public ActionResult GetPrivateConversationState([FromRoute] string channelId, [FromRoute] string conversationId, [FromRoute] string userId, [FromBody] BotState state)
        {
            _logger.LogDebug("GET /v3/botstate/:channelId/conversations/:conversationId/users/:userId");
            _logger.LogInformation("Getting PrivateConversations's bot state");
            var key = GetBotDataKey(channelId: channelId, conversationId:conversationId, userId: userId);
            return Ok(base.GetBotData(key));
        }

        [HttpPost]
        [Route("/v3/botstate/{channelId}/users/{userId}")]
        public ActionResult SetUserData([FromRoute] string channelId, [FromRoute] string userId, [FromBody] BotState state)
        {
            _logger.LogDebug("POST /v3/botstate/:channelId/users/:userId");
            _logger.LogInformation("Setting User's bot state");
            var key = GetBotDataKey(channelId: channelId, userId: userId);
            var newState = SetBotData(key, state);
            return Ok(newState);
        }

        [HttpPost]
        [Route("/v3/botstate/{channelId}/conversations/{conversationId}")]
        public ActionResult SetConversationData([FromRoute] string channelId, [FromRoute] string conversationId, [FromBody] BotState state)
        {
            _logger.LogDebug("POST /v3/botstate/:channelId/conversations/:conversationId");
            _logger.LogInformation("Setting Conversations's bot state");
            var key = GetBotDataKey(channelId: channelId, conversationId: conversationId);
            var newState = SetBotData(key, state);
            return Ok(newState);
        }

        [HttpPost]
        [Route("/v3/botstate/{channelId}/conversations/{conversationId}/users/{userId}")]
        public ActionResult SetPrivateConversationData([FromRoute] string channelId, [FromRoute] string conversationId, [FromRoute] string userId, [FromBody] BotState state)
        {
            _logger.LogDebug("POST /v3/botstate/:channelId/conversations/:conversationId/users/:userId");
            _logger.LogInformation("Setting PrivateConversations's bot state");
            var key = GetBotDataKey(channelId: channelId, conversationId: conversationId, userId:userId);
            var newState = SetBotData(key, state);
            return Ok(newState);
        }

        [HttpDelete]
        [Route("/v3/botstate/{channelId}/users/{userId}")]
        public ActionResult DeleteUserData([FromRoute] string channelId, [FromRoute] string userId, [FromBody] BotState state)
        {
            _logger.LogDebug("DELETE /v3/botstate/:channelId/users/:userId");
            _logger.LogInformation("Deleting User's bot state");
            var key = GetBotDataKey(channelId: channelId, userId: userId);
            DeleteStateForUser(userId);
            return Ok();
        }
    }
}