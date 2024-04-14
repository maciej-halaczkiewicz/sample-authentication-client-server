namespace simple_authentication_client_application.Common;

public static class StringExtensions
{
    public static string CalculateMD5(this string input)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        return Convert.ToHexString(hashBytes);
    }

    public static bool IsNotNullOrWhiteSpace(this string input)
    {
        return !string.IsNullOrWhiteSpace(input);
    }
}