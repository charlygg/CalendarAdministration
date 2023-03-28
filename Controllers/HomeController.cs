using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using CalendarAdministration.Models;
using CalendarAdministration.Services;
using System.Diagnostics;

namespace CalendarAdministration.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProfileService _profileService;

        public HomeController(ILogger<HomeController> logger,
            IProfileService profileService)
        {
            _logger = logger;
            _profileService = profileService;
        }

        [AuthorizeForScopes(Scopes = new string[] { "user.read", "Mail.Read" })]
        public async Task<IActionResult> Index()
        {
            var profile = await _profileService.GetProfileAsync();

            ViewData["UserName"] = profile?.DisplayName.ToString();
            ViewData["UserEmail"] = profile?.Mail.ToString();

            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}