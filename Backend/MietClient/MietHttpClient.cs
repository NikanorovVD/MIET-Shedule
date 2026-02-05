using MietClient.Extensions;
using System.Runtime.Serialization;
using System.Text.Json;

namespace MietClient
{
    public class MietHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _scheduleUrl;
        private readonly string _groupsUrl;
        private readonly MietCookies _cookies;

        public MietHttpClient(HttpClient httpClient, MietClientSettings settings)
            : this(httpClient, settings.ScheduleUrl, settings.GroupsUrl, settings.Cookies) { }

        public MietHttpClient(HttpClient httpClient, string scheduleUrl, string groupsUrl, MietCookies cookies)
        {
            _httpClient = httpClient;
            _scheduleUrl = scheduleUrl;
            _groupsUrl = groupsUrl;
            _cookies = cookies;
        }

        public async Task<IEnumerable<MietPair>> GetMietPairsAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<string> groups = await GetMietGroupsAsync(cancellationToken);
            IEnumerable<MietPair> couples = await groups.SelectManyAsync(g => GetGroupScheduleAsync(g, cancellationToken));

            return couples;
        }

        private async Task<IEnumerable<MietPair>> GetGroupScheduleAsync(string group, CancellationToken cancellationToken = default)
        {
            string fullUrl = $"{_scheduleUrl}/{group}";
            HttpRequestMessage request = new(HttpMethod.Post, fullUrl);
            request.Headers.Add("Cookie", _cookies.CookiesString);
            request.Content = new FormUrlEncodedContent(
            [
                new("group", group)
            ]);

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Error requesting {fullUrl}", null, statusCode: response.StatusCode);

            MietGroupSchedule groupSchedule = await JsonSerializer.DeserializeAsync<MietGroupSchedule>(response.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken)
                ?? throw new SerializationException($"Deserialization fail for request: {fullUrl}");

            return groupSchedule.Data;
        }

        private async Task<IEnumerable<string>> GetMietGroupsAsync(CancellationToken cancellationToken = default)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _groupsUrl);
            request.Headers.Add("Cookie", _cookies.CookiesString);

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Error requesting {_groupsUrl}", null, statusCode: response.StatusCode);


            IEnumerable<string> allGroups = await JsonSerializer.DeserializeAsync<IEnumerable<string>>(response.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken)
                 ?? throw new SerializationException($"Deserialization fail for request: {_groupsUrl}");

            IEnumerable<string> mietGroups = ExludeCollegeGroups(allGroups);
            return mietGroups;
        }

        private IEnumerable<string> ExludeCollegeGroups(IEnumerable<string> groups)
        {
            return groups.Where(gr => !IsCollegeGroup(gr));

            bool IsCollegeGroup(string gr)
                => (gr.EndsWith('О') || gr.EndsWith('С') || gr.Contains("колледж", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
