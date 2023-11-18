namespace SettingsManager.Models;

/// <summary>
/// This interface is used to differentiate your class as a config
/// for use with <see cref="SettingsManagerService"/>. A class should
/// not implement both <see cref="ISetting"/> and <see cref="IConfig"/>,
/// but may implement both <see cref="ISetting"/> and <see cref="IFeature"/>
///
/// Configurations are effectively settings that apply to the entire application
/// and do not differ between customers. An example may be connection information
/// for an global integration.
/// </summary>
public interface IConfig { }