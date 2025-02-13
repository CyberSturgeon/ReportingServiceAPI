using ReportingService.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Core.Configuration;

public class CustomerFilter
{
    public TransactionFilterForCustomer? TransactionFilter {  get; set; }
    public AccountFilterForCustomer? AccountFilter { get; set; }
    public BdayFilterForCustomer? BdayFilter { get; set; }
}
