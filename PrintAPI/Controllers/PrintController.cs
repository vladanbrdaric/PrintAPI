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
    public class PrintController : ControllerBase
    {
        [HttpGet]
        public async Task<PrintJobFullModel> GetPrintJob()
        {
            PrintJobFullModel printJob = new PrintJobFullModel();
            printJob = await GetFromDatabase();

            return printJob;
        }

        private async Task<PrintJobFullModel> GetFromDatabase()
        {
            PrintJobFullModel printJob = new PrintJobFullModel()
            {
                PrinterName = "printer1",
                IpAddress = "10.10.13.10",
                Port = "631",
                Protocol = "IIP",
                FileToBePrinted = "file1",
            };

            return printJob;
        }



        [HttpPost]
        public async Task ProcessPrintJobStatusInfo(PrintJobFullModel printJob)
        {
            await PrintPrintJobStatus(printJob);
        }

        private async Task PrintPrintJobStatus(PrintJobFullModel printJob)
        {
            Console.WriteLine(" ");
            Console.WriteLine("=== Answer from the agent  ===");
            Console.WriteLine($"Printer name: { printJob.PrinterName }, Printer job status: { printJob.PrintStatus }");
            Console.WriteLine("");
        }
    }
}
