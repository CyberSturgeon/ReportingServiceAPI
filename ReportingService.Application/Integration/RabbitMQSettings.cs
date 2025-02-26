
namespace ReportingService.Application.Integration;

public class RabbitMQSettings
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string TransactioncreatedQueue {  get; set; }
    public string AccountUpdateQueue { get; set; }
    public string CustomerWithAccountQueue {  get; set; }
    public string RoleUpdateQueue { get; set; }
    public string CustomerMessageQueue { get; set; }
}
