using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.Helpers;
using Newtonsoft.Json.Linq; 
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace job_buddy_backend.Core
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfigurationService _configurationService;
        private readonly ILogger<LocationService> _logger;

        public LocationService(HttpClient httpClient, IConfigurationService configurationService, ILogger<LocationService> logger)
        {
            _httpClient = httpClient;
            _configurationService = configurationService;
            _logger = logger;
        }

        public async Task<List<string>> GetCountriesAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://restcountries.com/v3.1/all");
                var countries = JArray.Parse(response).Select(c => c["name"]?["common"]?.ToString()).Where(c => !string.IsNullOrEmpty(c)).ToList();
                return countries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching countries from RESTCountries API.");
                throw;
            }
        }

        public async Task<List<string>> GetStatesAsync(string countryCode)
        {
            try
            {
                var geonamesUsername = await _configurationService.GetSettingAsync("GeonamesUsername");
                var response = await _httpClient.GetStringAsync($"http://api.geonames.org/searchJSON?country={countryCode}&featureCode=ADM1&maxRows=1000&username={geonamesUsername}");
                var states = JObject.Parse(response)["geonames"]?.Select(s => s["name"]?.ToString()).Where(s => !string.IsNullOrEmpty(s)).ToList();
                return states ?? new List<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching states for country code {countryCode} from Geonames API.");
                throw;
            }
        }

        public async Task<List<string>> GetCitiesAsync(string stateName)
        {
            try
            {
                var geonamesUsername = await _configurationService.GetSettingAsync("GeonamesUsername");
                var response = await _httpClient.GetStringAsync($"http://api.geonames.org/searchJSON?q={stateName}&featureCode=PPL&maxRows=1000&username={geonamesUsername}");
                var cities = JObject.Parse(response)["geonames"]?.Select(c => c["name"]?.ToString()).Where(c => !string.IsNullOrEmpty(c)).ToList();
                return cities ?? new List<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching cities for state {stateName} from Geonames API.");
                throw;
            }
        }

        public async Task<List<string>> GetZipCodesAsync(string cityName)
        {
            try
            {
                var geonamesUsername = await _configurationService.GetSettingAsync("GeonamesUsername");
                var response = await _httpClient.GetStringAsync($"http://api.geonames.org/postalCodeSearchJSON?placename={cityName}&maxRows=10&username={geonamesUsername}");
                var zipCodes = JObject.Parse(response)["postalCodes"]?.Select(z => z["postalCode"]?.ToString()).Where(z => !string.IsNullOrEmpty(z)).ToList();
                return zipCodes ?? new List<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching zip codes for city {cityName} from Geonames API.");
                throw;
            }
        }
    }
}
