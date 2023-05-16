using MarcosWebApiProject.ApplicationService.Interfaces;
using MarcosWebApiProject.ApplicationService.ADProduct;
using Microsoft.Extensions.Configuration;
using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI_API.Chat;
using System.Reflection.Metadata;

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

            if (apiModel.Contains("gpt"))
            {
                    var chatRequest = new ChatRequest()
                    {
                        Messages = new List<ChatMessage>()
                    {
                        new ChatMessage()
                        {
                            Content = generateRequestModel.prompt
                        }
                    },
                    Model = apiModel,
                    Temperature = 0.5,
                    MaxTokens = 100,
                    TopP = 1.0,
                    FrequencyPenalty = 0.0,
                    PresencePenalty = 0.0,

                };

                var result = await api.Chat.CreateChatCompletionAsync(chatRequest);

                foreach (var choice in result.Choices.DistinctBy(choice => choice.Message))
                {
                    rs = choice.Message.Content;
                    rq.Add(choice.Message.Content);

                }
            }
            else
            {
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
                foreach (var choice in result.Completions.DistinctBy(choice => choice.Text))
                {
                    rs = choice.Text;
                    rq.Add(choice.Text);
                }
            }

            return rq;
        }

    }
}
