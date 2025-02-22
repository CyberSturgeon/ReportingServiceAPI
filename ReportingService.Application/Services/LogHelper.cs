
namespace ReportingService.Application.Services;

internal static class LogHelper
{
    public static string HideEmail(string email)
    {
        var atIndex = email.IndexOf('@');
        if (atIndex <= 1)
        {
            return email;
        }

        var maskedEmail = email.Substring(0, 1) + new string('*', atIndex - 1) + email.Substring(atIndex);
        return maskedEmail;
    }
}
