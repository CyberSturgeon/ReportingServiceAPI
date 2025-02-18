using System.Net;

namespace ReportingService.Application.Exceptions;

public class CustomException(string message, HttpStatusCode code) : Exception(message)
{
    public readonly HttpStatusCode StatusCode = code;
}
