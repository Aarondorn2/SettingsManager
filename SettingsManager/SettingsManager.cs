using System.Text.Json;
using SettingsManager.SettingsManagerProviders;

namespace SettingsManager;

/// <summary>
/// This class is effectively a dependency injection wrapper. The <see cref="Init"/> method is required
/// to be called once when your application starts. If your application uses dependency injection, you can
/// collect the required dependencies from the service provider and provide them to <see cref="Init"/>,
/// otherwise, you can manually build the required classes needed to run SettingsManager.
/// </summary>
public static class SettingsManager
{
    /// <summary>
    /// The <see cref="IPersistenceProvider"/> that will be used by the SettingsManager library.
    /// </summary>
    private static IPersistenceProvider? PersistenceProvider { get; set; }

    /// <summary>
    /// A utility method to retrieve the currently configured <see cref="IPersistenceProvider"/> in use by the
    /// SettingsManager library.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    internal static IPersistenceProvider GetPersistenceProvider()
    {
        if (PersistenceProvider == null) throw new Exception("PersistenceProvider is null; Invoke `SettingsManager.Init()` before use.");
        return PersistenceProvider;
    }
    
    /// <summary>
    /// The <see cref="ICryptographyProvider"/> that will be used by the SettingsManager library.
    /// </summary>
    private static ICryptographyProvider? CryptographyProvider { get; set; }

    /// <summary>
    /// A utility method to retrieve the currently configured <see cref="ICryptographyProvider"/> in use by the
    /// SettingsManager library.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    internal static ICryptographyProvider GetCryptographyProvider()
    {
        if (CryptographyProvider == null) throw new Exception("CryptographyProvider is null; Invoke `SettingsManager.Init()` before use.");
        return CryptographyProvider;
    }
    
    /// <summary>
    /// <see cref="JsonSerializerOptions"/> used to serialize and deserialize objects that are sent to and
    /// retrieved from the <see cref="SettingsManagerProviders.IPersistenceProvider"/>.
    /// </summary>
    private static JsonSerializerOptions? SerializerOptions { get; set; }
    
    /// <summary>
    /// A utility method to retrieve the currently configured <see cref="JsonSerializerOptions"/> in use by the
    /// SettingsManager library.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    internal static JsonSerializerOptions GetSerializerOptions()
    {
        if (SerializerOptions == null) throw new Exception("SerializerOptions is null; Invoke `SettingsManager.Init()` before use.");
        return SerializerOptions;
    }
    
    /// <summary>
    /// This method must be called once when your application starts. It is used to manage the dependencies in use
    /// by the SettingsManager library.
    /// </summary>
    /// <param name="persistenceProvider"></param>
    /// <param name="serializerOptions"></param>
    /// <param name="cryptographyProvider"></param>
    public static void Init(
        IPersistenceProvider persistenceProvider,
        JsonSerializerOptions serializerOptions,
        ICryptographyProvider? cryptographyProvider = null
    )
    {
        PersistenceProvider = persistenceProvider;
        SerializerOptions = serializerOptions;
        CryptographyProvider = cryptographyProvider;
    }
}
