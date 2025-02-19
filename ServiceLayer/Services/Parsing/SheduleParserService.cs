using DataLayer.Entities;
using ServiceLayer.Constants;
using ServiceLayer.Extensions;
using ServiceLayer.Models.Exceptions;
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
            request.Content = new FormUrlEncodedContent(
            [
                new("group", group)
            ]);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new MietRequestException("MIET server response with error code", fullUrl, response.StatusCode, await response.Content.ReadAsStringAsync());      

            try
            {
                MietGroupShedule groupShedule = (await JsonSerializer.DeserializeAsync<MietGroupShedule>(response.Content.ReadAsStream()))!;
                return groupShedule.Data;
            }
            catch (Exception ex)
            {
                throw new MietRequestException($"Error while deserializing MIET server response: {ex}", fullUrl, response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        private async Task<IEnumerable<string>> GetMietGroupsAsync()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _groupsUrl);
            request.Headers.Add(MietCookies.CookiesHeader, MietCookies.CookiesString);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new MietRequestException("MIET server response with error code", _groupsUrl, response.StatusCode, await response.Content.ReadAsStringAsync());

            try 
            {
                IEnumerable<string> groups = (await JsonSerializer.DeserializeAsync<IEnumerable<string>>(response.Content.ReadAsStream()))!;
                return groups;
            }
            catch (Exception ex)
            {
                throw new MietRequestException($"Error while deserializing MIET server response: {ex}", _groupsUrl, response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }       
    }
}
