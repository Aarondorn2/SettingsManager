using System.Text.Json;
using System.Text.Json.Serialization;

namespace Noogadev.SettingsManager;

/// <summary>
/// Some setting and config properties are sensitive and should not be persisted in plain text.
/// This JsonConverter can be used to encrypt/decrypt during serialization/deserialization.
///
/// To use, simple annotate any property of your setting/config with:
///
/// [JsonConverter(typeof(EncryptedProperty{T}))]
///
/// where T is the type of the property you are annotating.
///
/// For an example, <see cref="Models.SettingsManagerSettingExample"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public class EncryptedProperty<T> : JsonConverter<T?>
{
    /// <summary>
    /// This converter supports all types.
    /// </summary>
    /// <param name="_"></param>
    /// <returns></returns>
    public override bool CanConvert(Type _) => true;
    
    /// <summary>
    /// When reading a value, it is assumed that the value was previously written with this converter.
    /// Therefore, the value is of type string and that string can be processed by the
    /// <see cref="SettingsManagerProviders.ICryptographyProvider"/> before deserialization
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="_"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override T? Read(ref Utf8JsonReader reader, Type _, JsonSerializerOptions options)
    {
        var crypto = SettingsManager.GetCryptographyProvider();
        var val = reader.GetString();
        if (val == null) return default;

        var decrypted = crypto.DecryptString(val);
        var deserialized = JsonSerializer.Deserialize<T>(decrypted, options);
        return deserialized;
    }

    /// <summary>
    /// When writing a value, the resulting type will always be a string, regardless of the source type.
    /// To facilitate this, the source type is serialized (unless it is already of type string) before
    /// being encrypted with the <see cref="SettingsManagerProviders.ICryptographyProvider"/>
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        var crypto = SettingsManager.GetCryptographyProvider();
        var val = value == null
            ? null
            : value as string ?? JsonSerializer.Serialize(value, options);
        
        if (val == null)
        {
            writer.WriteNullValue();
            return;
        }

        var encrypted = crypto.EncryptString(val);
        writer.WriteStringValue(encrypted);
    }
}
