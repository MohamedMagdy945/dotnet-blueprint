namespace Blueprint.Application.Common.Extensions;

public static class StringExtensions
{
    public static string NormalizeEmail(this string email)
    {
        return email.Trim().ToLowerInvariant();
    }
}
