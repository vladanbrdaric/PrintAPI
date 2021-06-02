using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrintAPI.Models
{
    public class PrinterInfoModel
    {
        public string Ip { get; set; }
        public string Community { get; set; }
        public List<OidModel> OidList { get; set; } = new List<OidModel>();
    }
}
