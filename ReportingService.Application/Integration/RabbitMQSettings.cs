﻿
namespace ReportingService.Application.Integration;

public class RabbitMQSettings
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string CustomerQueue {  get; set; }
    public string RoleUpdateQueue { get; set; }


}
