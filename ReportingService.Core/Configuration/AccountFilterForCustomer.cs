using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Core.Configuration;

public class AccountFilterForCustomer
{
    public required int? AccountsCount { get; set; }
    public required List<Currency>? Currencies { get; set; }
}
