using SV18T1021332.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021332.Web.Models
{
    public class EmployeePaginationResult : BasePaginationResult
    {
        public List<Employee> Data { get; set; }
    }
}