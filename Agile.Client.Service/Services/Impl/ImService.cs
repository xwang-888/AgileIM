using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Agile.Client.Service.Api;
using Agile.Client.Service.Api.Common;

using AgileIM.Shared.Models.ApiResult;
using AgileIM.Shared.Models.ClientModels.Message.Entity;

using Newtonsoft.Json;

namespace Agile.Client.Service.Services.Impl
{
    public class ImService : IImService
    {
        public async Task<Response<string>> SendMessage(Messages messages, bool isGroup = false)
        {
            var api = new SendMessageApi
            {
                FromId = messages.FromId,
                TargetId = messages.TargetId,
                Content = JsonConvert.SerializeObject(messages),
                MsgType = isGroup ? 1 : 0
            };
            return await api.GetRequest<Response<string>>();
        }
    }
}
