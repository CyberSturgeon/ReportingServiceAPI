using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Core.Configuration;

public class BdayFilterForCustomer
{
    public required DateTime DateStart {  get; set; }
    public required DateTime DateEnd { get; set; }
}
