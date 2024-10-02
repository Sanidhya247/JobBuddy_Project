using job_buddy_backend.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace job_buddy_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILogger<ConfigurationController> _logger;

        public ConfigurationController(IConfigurationService configurationService, ILogger<ConfigurationController> logger)
        {
            _configurationService = configurationService;
            _logger = logger;
        }

        // Get a specific configuration setting by key
        [HttpGet("{key}")]
        public async Task<IActionResult> GetSetting(string key)
        {
            try
            {
                var value = await _configurationService.GetSettingAsync(key);
                if (value == null)
                {
                    return NotFound($"Setting with key '{key}' not found.");
                }

                return Ok(new { Key = key, Value = value });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving setting with key '{key}'.");
                return StatusCode(500, "An error occurred while retrieving the setting.");
            }
        }

        // Get all settings
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSettings()
        {
            try
            {
                var settings = await _configurationService.GetAllSettingsAsync();
                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all settings.");
                return StatusCode(500, "An error occurred while retrieving settings.");
            }
        }

        // Create a new configuration setting
        [HttpPost("create")]
        public async Task<IActionResult> CreateSetting([FromBody] KeyValuePair<string, string> setting)
        {
            if (string.IsNullOrWhiteSpace(setting.Key) || string.IsNullOrWhiteSpace(setting.Value))
            {
                return BadRequest("Setting key and value cannot be null or empty.");
            }

            try
            {
                var created = await _configurationService.CreateSettingAsync(setting.Key, setting.Value);
                if (!created)
                {
                    return Conflict($"Setting with key '{setting.Key}' already exists.");
                }

                _logger.LogInformation($"Setting with key '{setting.Key}' created successfully.");
                return Ok($"Setting '{setting.Key}' created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating setting with key '{setting.Key}'.");
                return StatusCode(500, "An error occurred while creating the setting.");
            }
        }

        // Update an existing configuration setting
        [HttpPut("update")]
        public async Task<IActionResult> UpdateSetting([FromBody] KeyValuePair<string, string> setting)
        {
            if (string.IsNullOrWhiteSpace(setting.Key) || string.IsNullOrWhiteSpace(setting.Value))
            {
                return BadRequest("Setting key and value cannot be null or empty.");
            }

            try
            {
                await _configurationService.SetSettingAsync(setting.Key, setting.Value);
                _logger.LogInformation($"Setting with key '{setting.Key}' updated successfully.");
                return Ok($"Setting '{setting.Key}' updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating setting with key '{setting.Key}'.");
                return StatusCode(500, "An error occurred while updating the setting.");
            }
        }

        // Delete a configuration setting by key
        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteSetting(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return BadRequest("Setting key cannot be null or empty.");
            }

            try
            {
                var deleted = await _configurationService.DeleteSettingAsync(key);
                if (!deleted)
                {
                    return NotFound($"Setting with key '{key}' not found.");
                }

                _logger.LogInformation($"Setting with key '{key}' deleted successfully.");
                return Ok($"Setting '{key}' deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting setting with key '{key}'.");
                return StatusCode(500, "An error occurred while deleting the setting.");
            }
        }

        // Clear all cached settings
        [HttpPost("clear-cache")]
        public IActionResult ClearCache()
        {
            try
            {
                _configurationService.ClearCache();
                _logger.LogInformation("Configuration cache cleared successfully.");
                return Ok("Configuration cache cleared successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing configuration cache.");
                return StatusCode(500, "An error occurred while clearing the cache.");
            }
        }
    }
}
