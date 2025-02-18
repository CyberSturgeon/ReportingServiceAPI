
using System.Net;

namespace ReportingService.Application.Exceptions
{
    public class EntityNotFoundException(string message) : CustomException(message, HttpStatusCode.NotFound)
    { }
}
