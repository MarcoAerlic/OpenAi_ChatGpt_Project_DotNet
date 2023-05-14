using MarcosWebApiProject.ApplicationService.ADProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcosWebApiProject.ApplicationService.Interfaces
{
    public interface IBotAPIService
    {
        Task<List<string>> GenerateContent(ADGenerateRequestModelDTO generateRequestModel);
    }

}
