using MarcosWebApiProject.ApplicationService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcosWebApiProject.ApplicationService.ADProduct
{
    public class ADProductService : IADProductService
    {
        private readonly IBotAPIService _botAPIService;

        public ADProductService(IBotAPIService botAPIService)
        {
            _botAPIService = botAPIService;

        }

        public async Task<ADProductResponseModel> GenerateAdContent(CustomerRequestModel aDGenerateRequestModel)
        {
            if (string.IsNullOrEmpty(aDGenerateRequestModel.Message))
            {
                return new ADProductResponseModel
                {
                    Success = false,
                    ADContent = null
                };
            }
            var userMessage = new ADGenerateRequestModelDTO
            {
                prompt = aDGenerateRequestModel.Message
            };
            var generateAD = await _botAPIService.GenerateContent(userMessage);

            if (generateAD.Count == 0)
            {
                return new ADProductResponseModel
                {
                    Success = false,
                    ADContent = null
                };
            }

            return new ADProductResponseModel
            {
                Success = true,
                ADContent = generateAD
            };
        }
    }

}
