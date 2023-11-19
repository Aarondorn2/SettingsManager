namespace Noogadev.SettingsManager.Models;

/// <summary>
/// This class is utilized by <see cref="SettingsManagerService.IsFeatureEnabled{T}"/>
/// during deserialization.
/// </summary>
public class DefaultFeature : IFeature
{
    /// <summary>
    /// Whether the feature is enabled of disabled
    /// </summary>
    public bool IsEnabled { get; set; }
}
