using DataLayer.Entities;
using Microsoft.Extensions.Options;
using MietClient;

namespace ServiceLayer.Services
{
    public class MietClientService
    {
        private readonly MietHttpClient _mietClient;
        public MietClientService(IHttpClientFactory httpClientFactory, IOptions<MietClientSettings> options)
        {
            _mietClient = new(httpClientFactory.CreateClient(), options.Value);
        }

        public async Task<IEnumerable<MietPair>> GetMietPairsAsync(CancellationToken cancellationToken = default)
        {
            return await _mietClient.GetMietPairsAsync(cancellationToken);
        }
    }
}
