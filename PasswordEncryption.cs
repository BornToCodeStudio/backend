using System.Text;

namespace borntocode_backend;

public static class PasswordEncryption
{
    public static byte[] Encrypt(string input)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(input);
        byte[] bytes = null!;
        try
        {
            bytes = Encoding.ASCII.GetBytes(hash);
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Encryption] Не удалось получить байты. {e.Message}");
        }
            
        return bytes;
    }

    private static string GetHash(byte[] bytes)
    {
        var hash = string.Empty;
        try
        {
            hash = Encoding.ASCII.GetString(bytes);
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Encryption] Не удалось получить Hash. {e.Message}");
        }

        return hash;
    }

    public static bool CheckPassword(byte[] original, string input)
    {
        var originalHash = GetHash(original);

        return BCrypt.Net.BCrypt.Verify(input, originalHash);
    }
}