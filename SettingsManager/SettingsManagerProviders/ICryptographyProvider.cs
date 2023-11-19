namespace Noogadev.SettingsManager.SettingsManagerProviders;

/// <summary>
/// This interface provides a way for you to define how you would like to encrypt and
/// decrypt properties of settings. This only applies to properties annotated with
/// <see cref="EncryptedProperty{T}"/>. An AES implementation is already provided
/// (<see cref="AesCryptographyProvider"/>) and only requires you provide it with
/// your own symmetric key.
/// </summary>
public interface ICryptographyProvider
{
    /// <summary>
    /// This method should take a string, encrypt it, and return the encrypted value
    /// as a string. Any encoding or converting should be mirrored in the <see cref="DecryptString"/>
    /// method.
    /// </summary>
    /// <param name="stringToEncrypt"></param>
    /// <returns></returns>
    public string EncryptString(string stringToEncrypt);
    
    /// <summary>
    /// This method should take a string, decrypt it, and return the plain text value
    /// as a string. Any encoding or converting should be mirrored in the <see cref="EncryptString"/>
    /// method.
    /// </summary>
    /// <param name="encryptedString"></param>
    /// <returns></returns>
    public string DecryptString(string encryptedString);
}
