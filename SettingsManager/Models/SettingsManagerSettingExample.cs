using System.Text.Json.Serialization;

namespace Noogadev.SettingsManager.Models;

/// <summary>
/// This class is just provided as an example to show how settings (and configs)
/// can utilize <see cref="EncryptedProperty{T}"/>, nested structures, varying
/// struct and object types, and hard-coded default values.
/// </summary>
public class SettingsManagerSettingExample : ISetting
{
    public string SomeSetting { get; set; } = "In-code setting default";
    public string? SomeSettingNotDefaulted { get; set; }
    public int? SomeInt { get; set; }

    [JsonConverter(typeof(EncryptedProperty<string>))]
    public string SomeEncryptedString { get; set; } = "is it secret? is it safe?";

    [JsonConverter(typeof(EncryptedProperty<SettingsManagerEncryptedObjectExample>))]
    public SettingsManagerEncryptedObjectExample SomeEncryptedObject { get; set; } = new();
}

public class SettingsManagerEncryptedObjectExample
{
    public string SomeSetting { get; set; } = "hi mom";
}
