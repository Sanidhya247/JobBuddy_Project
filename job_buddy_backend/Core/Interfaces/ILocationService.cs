using System.Collections.Generic;
using System.Threading.Tasks;

namespace job_buddy_backend.Core.Interfaces
{
    public interface ILocationService
    {
        Task<List<string>> GetCountriesAsync();
        Task<List<string>> GetStatesAsync(string countryCode);
        Task<List<string>> GetCitiesAsync(string stateName);
        Task<List<string>> GetZipCodesAsync(string cityName);
    }
}
