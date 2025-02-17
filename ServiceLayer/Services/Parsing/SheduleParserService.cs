using DataLayer.Entities;
using ServiceLayer.Constants;
using ServiceLayer.Extensions;
using ServiceLayer.Models.Parser;
using System.Text.Json;

namespace ServiceLayer.Services.Parsing
{
    public class SheduleParserService
    {
        private readonly HttpClient _httpClient;
        private readonly MietSheduleAdapterService _adapterService;
        const string _sheduleUrl = @"https://www.miet.ru/schedule/data";
        const string _groupsUrl = @"https://www.miet.ru/schedule/groups";

        public SheduleParserService(HttpClient httpClient, MietSheduleAdapterService adapterService)
        {
            _httpClient = httpClient;
            _adapterService = adapterService;
        }

        public async Task<IEnumerable<Couple>> GetAdaptedCouplesAsync()
        {
            var mietCouples = await GetMietCouplesAsync();
            return mietCouples.Select(c => _adapterService.Adapt(c));
        }

        public async Task<IEnumerable<MietCouple>> GetMietCouplesAsync()
        {
            IEnumerable<string> groups = await GetMietGroupsAsync();
            IEnumerable<MietCouple> couples = await groups.SelectManyAsync(g => GetGroupSheduleAsync(g));
            return couples;
        }

        private async Task<IEnumerable<MietCouple>> GetGroupSheduleAsync(string group)
        {
            string fullUrl = $"{_sheduleUrl}/{group}";
            HttpRequestMessage request = new(HttpMethod.Post, fullUrl);
            request.Headers.Add(MietCookies.CookiesHeader, MietCookies.CookiesString);
            request.Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new("group", group)
            });

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"$MIET server response with status code {response.StatusCode} " +
                    $"at {fullUrl}; " +
                    $"with body {await response.Content.ReadAsStringAsync()}");

            try
            {
                MietGroupShedule groupShedule = await JsonSerializer.DeserializeAsync<MietGroupShedule>(response.Content.ReadAsStream())
                    ?? throw new Exception($"Fail to deserialize MIET server response at {fullUrl}; " +
                    $"response was {await response.Content.ReadAsStringAsync()}; " +
                    $"with status code {response.StatusCode}");

                return groupShedule.Data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Invalid MIET server response at {fullUrl}; " +
                    $"response was {await response.Content.ReadAsStringAsync()}; " +
                    $"with status code {response.StatusCode}; " +
                    $"with exeption: {ex.ToString()}");
            }
        }

        private async Task<IEnumerable<string>> GetMietGroupsAsync()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _groupsUrl);
            request.Headers.Add(MietCookies.CookiesHeader, MietCookies.CookiesString);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"$MIET server response with status code {response.StatusCode} " +
                    $"at {_groupsUrl}; " +
                    $"with body {await response.Content.ReadAsStringAsync()}");

            IEnumerable<string> groups = await JsonSerializer.DeserializeAsync<IEnumerable<string>>(response.Content.ReadAsStream())
                ?? throw new Exception($"Fail to deserialize MIET server response at {_groupsUrl}; " +
                $"response was {await response.Content.ReadAsStringAsync()}; " +
                $"with status code {response.StatusCode}");
            return groups;
        }
    }
}
