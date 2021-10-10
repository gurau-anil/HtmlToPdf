using HtmlToPdf.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace HtmlToPdf.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConverter _converter;
        public HomeController(ILogger<HomeController> logger, IConverter converter)
        {
            _logger = logger;
            _converter = converter;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult PrintHomePage()
        {
            string fileName = DateTime.UtcNow.AddMinutes(345).ToString("yyyyMMddHHmmssfff") + ".pdf";
            var url = $"{Request.Scheme}://{Request.Host}/home/index";
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings() { Top = 10 }
                    },
                Objects = {
                        new ObjectSettings()
                        {
                            Page = url,
                        },
                    }
            };
            byte[] pdfBytes = _converter.Convert(doc);
            return File(pdfBytes, "application/octet-stream", fileName);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
