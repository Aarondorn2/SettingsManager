namespace SettingsManager.Models;

/// <summary>
/// This interface is used to differentiate your class as a feature
/// for use with <see cref="SettingsManagerService"/>. A class may
/// implement both <see cref="ISetting"/> and <see cref="IFeature"/>
/// if it needs to be enable-able and store properties.
///
/// Features are used to toggle on and off varying parts of your application
/// based on customer
/// </summary>
public interface IFeature
{
    /// <summary>
    /// Whether the feature is enabled or disabled. For more
    /// information: <see cref="SettingsManagerService.IsFeatureEnabled{T}"/>
    /// </summary>
    public bool IsEnabled { get; set; }
}
