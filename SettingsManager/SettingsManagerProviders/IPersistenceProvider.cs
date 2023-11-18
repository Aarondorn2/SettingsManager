namespace SettingsManager.SettingsManagerProviders;

/// <summary>
/// This interface provides a way for you to define how and where your settings
/// should be stored. Although a database is the recommended approach, any long-term
/// persistence technology should provide value.
///
/// A <see cref="LocalPersistenceProvider"/> is provided in this project as a brief
/// example and as a means to test functionality locally as needed.
///
/// If you are utilizing a database or similar networked component for persistence,
/// it may be beneficial to additionally apply some form of caching. Caching can
/// be applied directly in your implementation of this interface.
/// </summary>
public interface IPersistenceProvider
{
    /// <summary>
    /// This method should retrieve a string from storage based on the compound key:
    /// <see cref="key"/> + <see cref="customerId"/>
    ///
    /// If the string does not exist in storage, a null should be returned.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="customerId"></param>
    /// <returns></returns>
    public Task<string?> TryGetValue(string key, Guid customerId);
    
    /// <summary>
    /// This method should store or update storage for the compound key:
    /// <see cref="key"/> + <see cref="customerId"/>
    ///
    /// Depending on your use-case, it may also be important to handle concurrency
    /// and clobbering concerns inside of this method.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="customerId"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public Task AddOrUpdateSetting(string key, Guid customerId, string value);
}
