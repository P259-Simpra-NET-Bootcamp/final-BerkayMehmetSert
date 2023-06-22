namespace Core.Utilities.Code;

public static class CouponCodeGenerator
{
    private const string AllowedCharacters  = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int CodeLength  = 8;
    public static string GenerateCouponCode()
    {
        var random = new Random();
        var codeChars = new char[CodeLength];
        
        for (var i = 0; i < CodeLength; i++)
        {
            codeChars[i] = AllowedCharacters[random.Next(AllowedCharacters.Length)];
        }

        return new string(codeChars);
    }
}