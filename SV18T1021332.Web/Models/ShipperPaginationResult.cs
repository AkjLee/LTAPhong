using SV18T1021332.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021332.Web.Models
{
    public class ShipperPaginationResult : BasePaginationResult
    {
        public List<Shipper> Data { get; set; }
    }
}