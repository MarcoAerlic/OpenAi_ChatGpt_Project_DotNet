using MarcosWebApiProject.ApplicationService.Interfaces;
using MarcosWebApiProject.ApplicationService.ADProduct;
using Microsoft.Extensions.Configuration;
using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcosWebApiProject.Infrastructure.Network
{
    public class BotAPIService : IBotAPIService
    {
        private readonly IConfiguration _configuration;

        public BotAPIService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<string>> GenerateContent(ADGenerateRequestModelDTO generateRequestModel)
        {
            var apiKey = _configuration.GetSection("Appsettings:GChatAPIKEY").Value;
            var apiModel = _configuration.GetSection("Appsettings:Model").Value;
            List<string> rq = new List<string>();
            string rs = "";
            OpenAIAPI api = new OpenAIAPI(new APIAuthentication(apiKey));
            var completionRequest = new OpenAI_API.Completions.CompletionRequest()
            {
                Prompt = generateRequestModel.prompt,
                Model = apiModel,
                Temperature = 0.5,
                MaxTokens = 100,
                TopP = 1.0,
                FrequencyPenalty = 0.0,
                PresencePenalty = 0.0,

            };
            var result = await api.Completions.CreateCompletionsAsync(completionRequest);
            foreach (var choice in result.Completions)
            {
                rs = choice.Text;
                rq.Add(choice.Text);
            }
            return rq;
        }

    }
}
