using System.Text.Json;
using Noogadev.SettingsManager.Models;

namespace Noogadev.SettingsManager;

/// <summary>
/// This implementation provides all the functionality needed to interact with settings, configs,
/// and features. It utilizes <see cref="SettingsManagerProviders.IPersistenceProvider"/> to store
/// and retrieve settings and it manages serialization.
/// </summary>
public class SettingsManagerService : ISettingsManagerService
{
    /// <summary>
    /// A local reference to the globalId from <see cref="ISettingsManagerService"/>
    /// </summary>
    public static readonly Guid GlobalId = ISettingsManagerService.GlobalId;
    
    /// <summary>
    /// This method retrieves a setting from the persistence provider. If a setting of the
    /// provided type <see cref="T"/> does not exist for the provided <see cref="customerId"/>,
    /// then a `null` will be returned.
    /// </summary>
    /// <param name="customerId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T?> GetSetting<T>(Guid customerId) where T : class, ISetting
    {
        var persistence = SettingsManager.GetPersistenceProvider();
        var key = GetKey<T>();
        
        var serialized = await persistence.TryGetValue(key, customerId);
        
        return serialized == null
            ? null
            : JsonSerializer.Deserialize<T>(serialized, SettingsManager.GetSerializerOptions());
    }

    /// <summary>
    /// This method retrieves a setting from the persistence provider. If a setting of the
    /// provided type <see cref="T"/> does not exist for the provided <see cref="customerId"/>,
    /// then this method will fallback to the <see cref="GlobalId"/> and load the global setting.
    /// Global settings are required, so an exception is thrown if the global setting does not
    /// exist in the persistence provider.
    /// </summary>
    /// <param name="customerId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception">
    /// Thrown if there is no global setting for type <see cref="T"/> in the persistence provider
    /// </exception>
    public async Task<T> GetSettingOrDefault<T>(Guid? customerId) where T : class, ISetting
    {
        var clientSetting = customerId == null
            ? null
            : await GetSetting<T>(customerId.Value);

        if (clientSetting != null) return clientSetting;
        
        var globalSetting = await GetSetting<T>(GlobalId);
        if (globalSetting == null) throw new Exception($"Global setting was not set for key: {GetKey<T>()}");

        return globalSetting;
    }

    /// <summary>
    /// Adds a provided setting to the persistence provider if one does not already exist.
    /// If one does already exist, it will be replaced with a new object. Concurrency concerns
    /// should be handled in the implementation of <see cref="SettingsManagerProviders.IPersistenceProvider"/>
    /// </summary>
    /// <param name="setting"></param>
    /// <param name="customerId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task AddOrUpdateSetting<T>(T setting, Guid customerId) where T : class, ISetting
        => AddOrUpdate(setting, customerId);

    /// <summary>
    /// Checks to see if an <see cref="Models.IFeature"/> is enabled for the given <see cref="customerId"/>.
    /// If the feature is not persisted, or it is persisted with <see cref="IFeature.IsEnabled"/> set to false,
    /// then this method returns false.
    /// </summary>
    /// <param name="customerId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<bool> IsFeatureEnabled<T>(Guid customerId) where T : class, IFeature
    {
        var persistence = SettingsManager.GetPersistenceProvider();
        var key = GetKey<T>();
        
        var serialized = await persistence.TryGetValue(key, customerId);
        return serialized != null
            && JsonSerializer.Deserialize<DefaultFeature>(serialized, SettingsManager.GetSerializerOptions())?.IsEnabled == true;
    }

    /// <summary>
    /// Adds a provided feature to the persistence provider if one does not already exist.
    /// If one does already exist, it will be replaced with a new object. Concurrency concerns
    /// should be handled in the implementation of <see cref="SettingsManagerProviders.IPersistenceProvider"/>
    /// </summary>
    /// <param name="feature"></param>
    /// <param name="customerId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task AddOrUpdateFeature<T>(T feature, Guid customerId) where T : class, IFeature
        => AddOrUpdate(feature, customerId);

    /// <summary>
    /// This method retrieves a config from the persistence provider. Configs are meant to be
    /// global / customer agnostic and so are persisted with the <see cref="GlobalId"/> as a key.
    /// 
    /// Configs are required, so an exception is thrown if the config does not exist in the
    /// persistence provider.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception">
    /// Thrown if there is no config for type <see cref="T"/> in the persistence provider
    /// </exception>
    public async Task<T> GetConfig<T>() where T : class, IConfig
    {
        var persistence = SettingsManager.GetPersistenceProvider();
        var key = GetKey<T>();
        
        var serialized = await persistence.TryGetValue(key, GlobalId);
        
        var config = serialized == null
            ? null
            : JsonSerializer.Deserialize<T>(serialized, SettingsManager.GetSerializerOptions());
        if (config == null) throw new Exception($"Global config was not set for key: {key}");

        return config;
    }
    
    /// <summary>
    /// Adds a provided config to the persistence provider if one does not already exist.
    /// If one does already exist, it will be replaced with a new object. Concurrency concerns
    /// should be handled in the implementation of <see cref="SettingsManagerProviders.IPersistenceProvider"/>
    /// </summary>
    /// <param name="config"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task AddOrUpdateConfig<T>(T config) where T : class, IConfig
        => AddOrUpdate(config, GlobalId);
    
    /// <summary>
    /// A private method to centralize the AddOrUpdate logic across setting types.
    /// </summary>
    /// <param name="setting"></param>
    /// <param name="customerId"></param>
    /// <typeparam name="T"></typeparam>
    private static async Task AddOrUpdate<T>(T setting, Guid customerId) where T : class
    {
        var persistence = SettingsManager.GetPersistenceProvider();
        var key = GetKey<T>();
        var newVal = JsonSerializer.Serialize(setting, SettingsManager.GetSerializerOptions());

        await persistence.AddOrUpdateSetting(key, customerId, newVal);
    }

    /// <summary>
    /// Generates a string key from a class type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static string GetKey<T>() where T : class
        => typeof(T).FullName ?? typeof(T).Name;
}
