using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrintAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetPrinterController : ControllerBase
    {
        [HttpGet]
        public async Task<PrinterInfoModel> GetPrinterUpTimeInfo()
        {
            PrinterInfoModel printer = new PrinterInfoModel();
            printer = await GetFromDatabase();

            return printer;
        }

        private async Task<PrinterInfoModel> GetFromDatabase()
        {
            PrinterInfoModel printer = new PrinterInfoModel()
            {
                Ip = "10.10.13.10",
                Community = "public",
                OidList = new List<OidModel>
                {
                    new OidModel { Name = "DeviceSystemName", OidNumber = "1.3.6.1.2.1.1.5.0" },
                    new OidModel { Name = "DeviceUpTime", OidNumber = "1.3.6.1.2.1.1.3.0" }
                }
            };

            return printer;
        }


        [HttpPost]
        public async Task ProcessPrinterSnmpInfo(PrinterInfoModel printer)
        {
            await SnmpInfo(printer);
        }

        private async Task SnmpInfo(PrinterInfoModel printer)
        {
            Console.WriteLine(" ");
            Console.WriteLine("=== Answer from the agent  ===");

            foreach (var a in printer.OidList)
            {
                Console.WriteLine($"Requested: { a.Name }");
                Console.WriteLine($"Response: { a.Response }");
            }
            Console.WriteLine("");
        }
    }
}
