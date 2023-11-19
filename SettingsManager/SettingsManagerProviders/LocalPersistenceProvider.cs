using System.Collections.Concurrent;

namespace Noogadev.SettingsManager.SettingsManagerProviders;

/// <summary>
/// This class provides an implementation of <see cref="IPersistenceProvider"/> that simply
/// stores and retrieves things from an in-memory dictionary. This type of persistence will
/// not likely benefit your application and is provided as an example and means of testing
/// locally.
/// </summary>
public class LocalPersistenceProvider : IPersistenceProvider
{
    /// <summary>
    /// A dictionary to hold all of our settings
    /// </summary>
    private readonly ConcurrentDictionary<(string key, Guid customerId), string> _localSettings = new();
    
    /// <summary>
    /// A method to retrieve settings from the in-memory dictionary
    /// </summary>
    /// <param name="key"></param>
    /// <param name="customerId"></param>
    /// <returns></returns>
    public Task<string?> TryGetValue(string key, Guid customerId)
    {
        var result = _localSettings.TryGetValue((key, customerId), out var val) ? val : null;
        return Task.FromResult(result);
    }

    /// <summary>
    /// A method that adds a setting to the in-memory dictionary if one does not already
    /// exist. If the <see cref="key"/> + <see cref="customerId"/> already exist in the
    /// dictionary, the record is then updated with the provided <see cref="value"/>.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="customerId"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public Task AddOrUpdateSetting(string key, Guid customerId, string value)
    {
        _localSettings.AddOrUpdate((key, customerId), _ => value, (_, _) => value);
        return Task.CompletedTask;
    }

    /// <summary>
    /// A method to add testing data to the in-memory dictionary in bulk
    /// </summary>
    /// <param name="settings"></param>
    public void Seed(Dictionary<(string key, Guid customerId), string> settings)
    {
        foreach (var kvp in settings)
        {
            _localSettings.AddOrUpdate(kvp.Key, _ => kvp.Value, (_, _) => kvp.Value);
        }
    }
}
