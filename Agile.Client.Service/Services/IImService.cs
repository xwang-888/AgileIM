using AgileIM.Shared.Models.ClientModels.Message.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgileIM.Shared.Models.ApiResult;

namespace Agile.Client.Service.Services
{
    public interface IImService
    {
        Task<Response<string>> SendMessage(Messages messages,bool isGroup=false);
    }
}
