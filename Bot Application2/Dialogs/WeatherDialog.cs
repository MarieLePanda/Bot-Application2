using Bot_Application2.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application2.Dialogs
{
    [Serializable]
    public class WeatherDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as IMessageActivity;

            // TODO: Put logic for handling user message here
            string endpoint = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/6767436a-1094-4de9-9e0f-cc55030ab265?subscription-key=05ea02c6522e40b7a1c39931306f5e39&verbose=true&timezoneOffset=0&q=";
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
//            JSONResult.topScoringIntent.intent

            string endpointWeather = "api.openweathermap.org/data/2.5/weather?q=";
            queryParameter = JSONResult.entities.First().entity;
            string endQuery = "&APPID=143a6ad9ed21e367e1e9161d119b906e";
            RootObject weatherResult = null;
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
                            weatherResult = JsonConvert.DeserializeObject<RootObject>(stringResult);
                        }
                    }
                }
            }

            await context.PostAsync("A " + queryParameter + " il fait todo");

            //await context.PostAsync("Voici la météo " + weatherResult.weather.First().description);

            context.Done(activity);
        }
    }
}