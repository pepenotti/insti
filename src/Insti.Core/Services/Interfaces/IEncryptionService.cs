namespace Insti.Core.Services.Interfaces
{
    public interface IEncryptionService
    {
        string EncryptString(string text);
        string DecryptString(string cipherText);
    }
}