using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace RetailBot.Dialog
{
    [LuisModel("d91dbfa4-dcff-49bb-8d33-5371a25cdadd", "6d35713e91ae40859618c38a6ceb95c5")]
    [Serializable]
    public class RetailDialogBox : LuisDialog<object>
    {
        private UserData From = null;
        private UserData To = null;

        public RetailDialogBox(UserData from, UserData to)
        {
            From = from;
            To = to;
        }
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry I did not understand. Ask me questions like \"What events are on in Seattle?\" or manage your current events with \"Show me the events I have supported as a Viewer.\".");
            context.Wait(MessageReceived);
        }
        private List<string> AddToGoodsList(IDialogContext context,string item)
        {
            List<string> goods = null;
            context.ConversationData.TryGetValue<List<string>>("goods", out goods);
            if (goods == null)
                goods = new List<string>();
            goods.Add(item);
            context.ConversationData.SetValue<List<string>>("goods", goods);

            return goods;
        }
        [LuisIntent("AskLocation")]
        public async Task AskLocationAsync(IDialogContext context, LuisResult result)
        {
            EntityRecommendation good = null;
            List<string> goods = null;

            if (result.TryFindEntity("商品", out good))
            {
                goods = AddToGoodsList(context, good.Entity);
            }
            
            await context.PostAsync($"you ({From.Name}) are asking location of {good.Entity}");
            //await context.PostAsync($"you ({From.Name}) have asked location of {string.Join(",",goods.ToArray())}");
            context.Wait(MessageReceived);
        }
        [LuisIntent("AskPrice")]
        public async Task AskPriceAsync(IDialogContext context, LuisResult result)
        {
            EntityRecommendation good = null;
            EntityRecommendation quantity = null;
            List<string> goods = null;
            
            if (result.TryFindEntity("商品", out good))
            {
                goods = AddToGoodsList(context, good.Entity);
            }
            result.TryFindEntity("number", out quantity);
            
            await context.PostAsync($"you ({From.Name}) are asking {good.Entity}");
            //await context.PostAsync($"you ({From.Name}) have asked {string.Join(",", goods.ToArray())}");
            context.Wait(MessageReceived);
        }
    }
}