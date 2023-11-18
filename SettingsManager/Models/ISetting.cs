namespace SettingsManager.Models;

/// <summary>
/// This interface is used to differentiate your class as a setting
/// for use with <see cref="SettingsManagerService"/>. A class should
/// not implement both <see cref="ISetting"/> and <see cref="IConfig"/>,
/// but may implement both <see cref="ISetting"/> and <see cref="IFeature"/>
///
/// Settings are collections of properties that affect how your application functions.
/// They can be different per customer and a global default can be defined so that you
/// need not configure each setting for each customer - you only need to do so when the
/// customer's configuration differs from the default.
/// </summary>
public interface ISetting { }
