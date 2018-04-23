using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application2.Dialogs
{
    [Serializable]
    public class NewOrderDialog : IDialog<object> 
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if (activity.Text.ToLower().Contains("over"))
            {
                context.Done(activity);
            }
            else
            {
                await context.PostAsync($"Ok you want {activity.Text}");
                await context.PostAsync("say what you want or 'over' to finish");
                context.Wait(MessageReceivedAsync);
            }
        }
    }

}