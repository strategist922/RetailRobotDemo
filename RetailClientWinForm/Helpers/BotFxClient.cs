using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.DirectLine.Models;

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
                    FromProperty = From,
                    ConversationId = token.ConversationId,
                    Text = msg
                });
                var messages = conversations.GetMessages(token.ConversationId);
                var reply = messages.Messages.OrderByDescending(m => m.Created).First();

                return reply.Text;
            }
            catch (Exception exp)
            {
                return $"Error:{exp.Message}";
            }
        }
    }
}
