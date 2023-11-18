namespace SettingsManager.Models;

/// <summary>
/// This class is utilized by <see cref="SettingsManagerService.IsFeatureEnabled{T}"/>
/// during deserialization.
/// </summary>
public class DefaultFeature : IFeature
{
    public bool IsEnabled { get; set; }
}
