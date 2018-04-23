using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Bot_Application2.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace Bot_Application2.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;


            // return our reply to the user
            //Multiple dialog 1
           if (activity.Text.ToLower().Contains("order"))
            {
                await context.Forward(new NewOrderDialog(), this.ResumeAfterNewOrderDialog, activity, CancellationToken.None);
            }
            else
            {
                await context.PostAsync("Hello, What do you want? Maybe order?");
                context.Wait(MessageReceivedAsync);
            }

            /*string endpoint = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/6767436a-1094-4de9-9e0f-cc55030ab265?subscription-key=05ea02c6522e40b7a1c39931306f5e39&verbose=true&timezoneOffset=0&q=";
            string queryParameter = activity.Text;
            LuisJSON JSONResult = null;
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint + queryParameter))
                {
                    //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string stringResult = await response.Content.ReadAsStringAsync();
                            JSONResult = JsonConvert.DeserializeObject<LuisJSON>(stringResult);
                        }
                    }
                }
            }

            if (JSONResult.topScoringIntent.intent.ToLower().Equals("askweather"))
            {
                await context.Forward(new WeatherDialog(), this.ResumeAfterWeatherDialog, activity, CancellationToken.None);
            }
            else
            {
                await context.PostAsync("Je ne peux vous donnez que la météo\nEssayer de demander quel temps il fait à Paris");
                context.Wait(MessageReceivedAsync);
            }*/
        }

        private async Task ResumeAfterNewOrderDialog(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            await context.PostAsync($"you said {activity.Text} Thanks for ordering");

            context.Wait(MessageReceivedAsync);
        }

        private async Task ResumeAfterWeatherDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
        }

    }
}