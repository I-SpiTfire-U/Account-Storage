using System.Security.Cryptography;
using Account_Storage.Utilities;

namespace Account_Storage;
internal static class AccountEncryption
{
    public static void EncryptFile(byte[] key, byte[] iv, string filePath)
    {
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        string tempFilePath = filePath + ".tmp";

        using (FileStream fsInput = new(filePath, FileMode.Open))
        using (FileStream fsOutput = new(tempFilePath, FileMode.Create)) 
        using (CryptoStream cs = new(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
            fsInput.CopyTo(cs);
        }

        Thread.Sleep(100);

        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.Move(tempFilePath, filePath);
        }
        catch (IOException e)
        {
            OtherUtilities.PrintErrorMessage($"[Encrypting] Error attempting to rename/delete temp file: {e.Message}");
            return;
        }
    }


    public static void DecryptFile(byte[] key, byte[] iv, string filePath)
    {
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        string tempFilePath = filePath + ".tmp";

        using (FileStream fsInput = new(filePath, FileMode.Open))
        using (FileStream fsOutput = new(tempFilePath, FileMode.Create))
        using (CryptoStream cs = new(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
        {
            cs.CopyTo(fsOutput);
        }

        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.Move(tempFilePath, filePath);
        }
        catch (IOException e)
        {
            OtherUtilities.PrintErrorMessage($"[Decrypting] Error attempting to rename/delete temp file: {e.Message}");
            return;
        }
    }

    public static bool AreKeyAndIVCorrect(string path, byte[] key, byte[] iv)
    {
        using Aes aes = Aes.Create();
        try
        {
            aes.Key = key;
            aes.IV = iv;

            using FileStream fsInput = new(path, FileMode.Open);
            using CryptoStream cs = new(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read);

            byte[] buffer = new byte[1];
            int bytesRead = cs.Read(buffer, 0, buffer.Length);

            return bytesRead > 0; 
        }
        catch (CryptographicException)
        {
            return false;
        }
        catch
        {
            return true;
        }
    }
}