

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace WT.Trigger.Application.Common.Interfaces
{
    public interface IInternalHttpService
    {
        Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationtoken, Dictionary<string, string> values = null);

        Task<TResponse> PostJosnAsync<TResponse>(string url, object request, CancellationToken cancellationtoken);
    }
}
