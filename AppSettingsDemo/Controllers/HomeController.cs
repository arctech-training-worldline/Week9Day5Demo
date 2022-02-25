using AppSettingsDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppSettingsDemo.Models.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AppSettingsDemo.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ConnectionStrings _connectionStrings;
        private readonly ShaktimanMartSettings _shaktimanMartSettings;

        // The basic way of accessing appsettings using IConfiguration service
        public HomeController(
            ILogger<HomeController> logger, 
            IConfiguration configuration,
            IOptions<ConnectionStrings> connectionStringsAccessor,
            IOptions<ShaktimanMartSettings> shaktimanMartSettingsAccessor
            )
        {
            _logger = logger;
            _configuration = configuration;

            _connectionStrings = connectionStringsAccessor.Value;
            _shaktimanMartSettings = shaktimanMartSettingsAccessor.Value;

        }

        public IActionResult Index()
        {
            OldWayOfAccessingAppSettings();

            NewWayOfAccessingAppSettings();

            return View();
        }

        private void NewWayOfAccessingAppSettings()
        {
            var connectionString = _connectionStrings.DefaultConnectionString;
            var x = _connectionStrings.MyOtherConnectionString;
            var minProductPrice = _shaktimanMartSettings.MinProductPrice;
            var maxProductPrice = _shaktimanMartSettings.MaxProductPrice;


        }

        private void OldWayOfAccessingAppSettings()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnectionString");
            var x = _configuration.GetConnectionString("MyOtherConnectionString");
            var minProductPrice = _configuration["ShaktimanMartSettings:MinProductPrice"];
            var maxProductPrice = _configuration["ShaktimanMartSettings:MaxProductPrice"];
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
