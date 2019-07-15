using Directline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Directline.Controllers
{
    public class ControllerCommon : ControllerBase
    {
        public const int expiresIn = 1800;
        public readonly IDataStorage _datastorage;
        private bool conversationInitRequired = true;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContext;

        public ControllerCommon(IHttpContextAccessor httpContext, IConfiguration config, IDataStorage datastorage)
        {
            _httpContext = httpContext;
            _datastorage = datastorage;
            _config = config;
        }

        protected string GetServiceUrl()
        {
            var request = _httpContext.HttpContext.Request;

            var absoluteUri = string.Concat(
                        request.Scheme,
                        "://",
                        request.Host.ToUriComponent(),
                        request.PathBase.ToUriComponent());

            return absoluteUri;
        }
        protected Conversation GetConversation(string conversationId)
        {
            Conversation conversation;
            if (!_datastorage.Conversations.TryGetValue(conversationId, out conversation) && !conversationInitRequired)
            {
                conversation = new Conversation() { ConversationId = conversationId };
                _datastorage.Conversations.Add(conversationId, conversation);
            }

            return conversation;
        }
        protected Activity CreateConversationUpdateActivity(string conversationId)
        {
            return new ConversationUpdateActivity()
            {
                ChannelId = "emulator",
                ServiceUrl = GetServiceUrl(),
                Conversation = new ConversationAccount { Id = conversationId, IsGroup = false },
                From = new ChannelAccount { Id = "offline-directline", Name = "Offline Directline Server" }
            };
        }

        protected string GetBotDataKey(string channelId = "*", string conversationId = "*", string userId = "*")
        {
            return $"${channelId}!{conversationId}!{userId}";
        }

        protected BotState GetBotData(string key)
        {
            var state = _datastorage.BotStates.ContainsKey(key) ? _datastorage.BotStates[key] : new BotState() { ETag = "*", Data = null };
            return state;
        }
        protected BotState SetBotData(string key, BotState oldState)
        {
            var newState = new BotState()
            {
                ETag = DateTime.Now.ToString("o"),
                Data = oldState?.Data
            };

            if (oldState != null)
            {
                _datastorage.BotStates[key] = newState;
            }
            else
            {
                _datastorage.BotStates.Remove(key);
                newState.ETag = "*";
            }

            return newState;
        }

        protected void DeleteStateForUser(string userId)
        {
            var toDelete = _datastorage.BotStates.Where(w => w.Key.EndsWith($"!{userId}")).ToList();
            foreach (var s in toDelete)
            {
                _datastorage.BotStates.Remove(s.Key);
            }
        }

        protected string GenerateJSONWebToken(UserModel userInfo)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim("Token", userInfo.Token)
            };
            
            return GenerateJSONWebToken(claims, expires:DateTime.Now.AddMinutes(120));            
        }
        protected string GenerateJSONWebToken(IEnumerable<Claim> claims, DateTime? expires)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> cl = new List<Claim>();
            cl.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            cl.AddRange(claims);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                cl.ToArray(),
                expires: expires,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
