using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.DirectLine.Models;
using System.Configuration;
using System.Net.Http;

namespace RetailClientWinForm.Helpers
{
    //http://blog.qaramell.com/papemk2/16871
    public class BotFxClient
    {
        private string Secret = null;
        private string Url = null;
        DirectLineClient client = null;
        Conversations conversations = null;
        Conversation token = null;
        public string From { get; set; }

        public BotFxClient(string botUrl, string directLineSecret)
        {
            Url = botUrl;
            Secret = directLineSecret;

            client = new DirectLineClient(
                                        new Uri(botUrl),
                                        new DirectLineClientCredentials(Secret));
            conversations = new Conversations(client);
            token = conversations.NewConversation();
        }


        
        public async Task<string> TalkAsync(string msg)
        {
            try
            {
                var o = conversations.PostMessage(token.ConversationId, new Message
                {
                    FromProperty = From == null ? "Michael SH Chi" : From,
                    //ChannelData = ConfigurationManager.AppSettings["RetailLocation"],
                    ConversationId = token.ConversationId,
                    Text = msg
                });
                try
                {
                    //var messages = await conversations.GetMessagesAsync(token.ConversationId);
                    var messages = conversations.GetMessages(token.ConversationId);
                    var reply = messages.Messages.OrderByDescending(m => m.Created).First();

                    //var messages = await conversations.GetMessagesWithHttpMessagesAsync(token.ConversationId);
                    //var reply = messages.Body.Messages.OrderByDescending(m => m.Created).FirstOrDefault();
                    return reply.Text;
                }
                catch(Exception exp)
                {
                    var a = exp;

                    return $"[Error]{exp.Message}";
                }

            }
            catch (Exception exp)
            {
                return $"Error:{exp.Message}";
            }
        }
    }
}
