using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace job_buddy_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<LocationController> _logger;

        public LocationController(ILocationService locationService, ILogger<LocationController> logger)
        {
            _locationService = locationService;
            _logger = logger;
        }

        // GET: api/location/countries
        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _locationService.GetCountriesAsync();
                return Ok(ApiResponse<List<string>>.SuccessResponse(countries, "Countries fetched successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching countries.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while fetching countries."));
            }
        }

        
        [HttpGet("states/{countryCode}")]
        public async Task<IActionResult> GetStates(string countryCode)
        {
            try
            {
                var states = await _locationService.GetStatesAsync(countryCode);
                return Ok(ApiResponse<List<string>>.SuccessResponse(states, "States fetched successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching states for country code {countryCode}.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while fetching states."));
            }
        }

        
        [HttpGet("cities/{stateName}")]
        public async Task<IActionResult> GetCities(string stateName)
        {
            try
            {
                var cities = await _locationService.GetCitiesAsync(stateName);
                return Ok(ApiResponse<List<string>>.SuccessResponse(cities, "Cities fetched successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching cities for state {stateName}.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while fetching cities."));
            }
        }

        
        [HttpGet("zipcode/{cityName}")]
        public async Task<IActionResult> GetZipCodes(string cityName)
        {
            try
            {
                var zipCodes = await _locationService.GetZipCodesAsync(cityName);
                return Ok(ApiResponse<List<string>>.SuccessResponse(zipCodes, "Zip codes fetched successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching zip codes for city {cityName}.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while fetching zip codes."));
            }
        }
    }
}
