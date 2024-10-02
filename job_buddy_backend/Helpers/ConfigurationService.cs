using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System;
using job_buddy_backend.Models.DataContext;
using Microsoft.EntityFrameworkCore;

namespace job_buddy_backend.Helpers
{
        public interface IConfigurationService
        {
        Task<string> GetSettingAsync(string key);           
        Task SetSettingAsync(string key, string value);      
        Task<Dictionary<string, string>> GetAllSettingsAsync(); 
        Task<bool> DeleteSettingAsync(string key);          
        Task<bool> CreateSettingAsync(string key, string value); 
        void ClearCache();
    }

        public class ConfigurationService : IConfigurationService
        {
            private readonly JobBuddyDbContext _context;
            private readonly ILogger<ConfigurationService> _logger;
            private readonly IMemoryCache _cache;
            private readonly IConfiguration _configuration;

        // To reduce db calls, store keys In-memory cache
        private static readonly ConcurrentDictionary<string, string> ConfigCache = new();

        public ConfigurationService(JobBuddyDbContext context, ILogger<ConfigurationService> logger, IMemoryCache cache, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
            _configuration = configuration;
        }

        // Retrieve a specific setting value by key
        public async Task<string> GetSettingAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;

            if (ConfigCache.TryGetValue(key, out var value))
            {
                return value;
            }

            var setting = await _context.AppSettings.SingleOrDefaultAsync(s => s.SettingKey == key);
            if (setting != null)
            {
                value = setting.SettingValue;
                ConfigCache[key] = value;
                _cache.Set(key, value, TimeSpan.FromMinutes(Convert.ToDouble(_configuration["AppSettings:ExpiryTimeInMin"] ?? "60")));
            }
            else
            {
                _logger.LogWarning($"Setting {key} not found in database.");
            }

            return value;
        }

        // Create a new setting (if not already existing)
        public async Task<bool> CreateSettingAsync(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) || value == null) return false;

            if (await _context.AppSettings.AnyAsync(s => s.SettingKey == key))
            {
                _logger.LogWarning($"Setting {key} already exists. Use UpdateSettingAsync instead.");
                return false;
            }

            _context.AppSettings.Add(new AppSetting { SettingKey = key, SettingValue = value });
            await _context.SaveChangesAsync();

            // Add to local cache
            ConfigCache[key] = value;
            _cache.Set(key, value, TimeSpan.FromMinutes(Convert.ToDouble(_configuration["AppSettings:ExpiryTimeInMin"] ?? "60")));

            return true;
        }

        // Update an existing setting or create it if it does not exist
        public async Task SetSettingAsync(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) || value == null) return;

            var existingSetting = await _context.AppSettings.SingleOrDefaultAsync(s => s.SettingKey == key);
            if (existingSetting == null)
            {
                await CreateSettingAsync(key, value);
            }
            else
            {
                existingSetting.SettingValue = value;
                _context.AppSettings.Update(existingSetting);
                await _context.SaveChangesAsync();

                // Update the cache
                ConfigCache[key] = value;
                _cache.Set(key, value, TimeSpan.FromMinutes(Convert.ToDouble(_configuration["AppSettings:ExpiryTimeInMin"] ?? "60")));
            }
        }

        // Delete a specific setting
        public async Task<bool> DeleteSettingAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return false;

            var setting = await _context.AppSettings.SingleOrDefaultAsync(s => s.SettingKey == key);
            if (setting == null)
            {
                _logger.LogWarning($"Setting {key} not found. Cannot delete.");
                return false;
            }

            _context.AppSettings.Remove(setting);
            await _context.SaveChangesAsync();

            // Remove from local cache
            ConfigCache.TryRemove(key, out _);
            _cache.Remove(key);

            return true;
        }

        // Retrieve all settings as a dictionary
        public async Task<Dictionary<string, string>> GetAllSettingsAsync()
        {
            var settings = await _context.AppSettings.ToDictionaryAsync(s => s.SettingKey, s => s.SettingValue);

            foreach (var setting in settings)
            {
                ConfigCache[setting.Key] = setting.Value;
                _cache.Set(setting.Key, setting.Value, TimeSpan.FromMinutes(Convert.ToDouble(_configuration["AppSettings:ExpiryTimeInMin"] ?? "60")));
            }

            return settings;
        }

        // Clear both local and in-memory cache
        public void ClearCache()
        {
            ConfigCache.Clear();
            foreach (var key in ConfigCache.Keys)
            {
                _cache.Remove(key);
            }

            _logger.LogInformation("Configuration cache cleared successfully.");
        }
    }

    
}
