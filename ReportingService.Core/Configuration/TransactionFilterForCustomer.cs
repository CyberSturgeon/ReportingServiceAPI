using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Application.Models;

public class TransactionFilterForCustomer
{
    public required int TransactionsCount { get; set; }
    public required DateTime DateStart { get; set; }
    public required DateTime DateEnd { get; set; }
}
