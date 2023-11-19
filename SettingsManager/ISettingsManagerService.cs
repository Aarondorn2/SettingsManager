using Noogadev.SettingsManager.Models;

namespace Noogadev.SettingsManager;

/// <summary>
/// This interface defines the methods available in the SettingsManagerService. These methods
/// standardize how settings, configs, and features are created, modified, and retrieved.
/// <see cref="SettingsManagerService"/> for summaries of each method
/// </summary>
public interface ISettingsManagerService
{
    /// <summary>
    /// This id is used as a global key when storing / retrieving settings, features, and configs.
    /// </summary>
    public static readonly Guid GlobalId = new Guid("11111111-61c8-4a18-8fe5-40ec9851cfa1");
    
    /// <summary>
    /// <see cref="SettingsManagerService.GetSetting{T}"/>
    /// </summary>
    /// <param name="customerId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task<T?> GetSetting<T>(Guid customerId) where T : class, ISetting;

    /// <summary>
    /// <see cref="SettingsManagerService.GetSettingOrDefault{T}"/>
    /// </summary>
    /// <param name="customerId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task<T> GetSettingOrDefault<T>(Guid? customerId) where T : class, ISetting;
    
    /// <summary>
    /// <see cref="SettingsManagerService.AddOrUpdateSetting{T}"/>
    /// </summary>
    /// <param name="setting"></param>
    /// <param name="customerId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task AddOrUpdateSetting<T>(T setting, Guid customerId) where T : class, ISetting;
    
    /// <summary>
    /// <see cref="SettingsManagerService.IsFeatureEnabled{T}"/>
    /// </summary>
    /// <param name="customerId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task<bool> IsFeatureEnabled<T>(Guid customerId) where T : class, IFeature;
    
    /// <summary>
    /// <see cref="SettingsManagerService.AddOrUpdateFeature{T}"/>
    /// </summary>
    /// <param name="feature"></param>
    /// <param name="customerId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task AddOrUpdateFeature<T>(T feature, Guid customerId) where T : class, IFeature;
    
    /// <summary>
    /// <see cref="SettingsManagerService.GetConfig{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task<T> GetConfig<T>() where T : class, IConfig;
    
    /// <summary>
    /// <see cref="SettingsManagerService.AddOrUpdateConfig{T}"/>
    /// </summary>
    /// <param name="config"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Task AddOrUpdateConfig<T>(T config) where T : class, IConfig;
}
