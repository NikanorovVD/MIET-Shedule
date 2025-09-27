using MietClient.Extensions;
using System.Runtime.Serialization;
using System.Text.Json;

namespace MietClient
{
    public class MietHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _sheduleUrl;
        private readonly string _groupsUrl;
        private readonly MietCookies _cookies;

        public MietHttpClient(HttpClient httpClient, MietClientSettings settings)
            : this(httpClient, settings.SheduleUrl, settings.GroupsUrl, settings.Cookies) { }

        public MietHttpClient(HttpClient httpClient, string sheduleUrl, string groupsUrl, MietCookies cookies)
        {
            _httpClient = httpClient;
            _sheduleUrl = sheduleUrl;
            _groupsUrl = groupsUrl;
            _cookies = cookies;
        }

        public async Task<IEnumerable<MietPair>> GetMietPairsAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<string> groups = await GetMietGroupsAsync(cancellationToken);
            IEnumerable<MietPair> couples = await groups.SelectManyAsync(g => GetGroupSheduleAsync(g, cancellationToken));

            return couples;
        }

        private async Task<IEnumerable<MietPair>> GetGroupSheduleAsync(string group, CancellationToken cancellationToken = default)
        {
            string fullUrl = $"{_sheduleUrl}/{group}";
            HttpRequestMessage request = new(HttpMethod.Post, fullUrl);
            request.Headers.Add("Cookie", _cookies.CookiesString);
            request.Content = new FormUrlEncodedContent(
            [
                new("group", group)
            ]);

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Error requesting {fullUrl}", null, statusCode: response.StatusCode);

            MietGroupShedule groupShedule = await JsonSerializer.DeserializeAsync<MietGroupShedule>(response.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken)
                ?? throw new SerializationException($"Deserialization fail for request: {fullUrl}");

            return groupShedule.Data;
        }

        private async Task<IEnumerable<string>> GetMietGroupsAsync(CancellationToken cancellationToken = default)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _groupsUrl);
            request.Headers.Add("Cookie", _cookies.CookiesString);

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Error requesting {_groupsUrl}", null, statusCode: response.StatusCode);


            IEnumerable<string> groups = await JsonSerializer.DeserializeAsync<IEnumerable<string>>(response.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken)
                 ?? throw new SerializationException($"Deserialization fail for request: {_groupsUrl}");

            return groups;
        }
    }
}
