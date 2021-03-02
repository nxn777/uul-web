using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uul_web.Pages.Login;

namespace uul_web.Controllers {
    [Route("[controller]/[action]")]
    public class AccountController : Controller {
        private readonly ILogger _logger;

        public AccountController(ILogger<AccountController> logger) {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Logout() {
            _logger.LogInformation("User logged out at {Time}.",DateTime.UtcNow);

  
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    

            return RedirectToPage("/Auth/SignedOut");
        }

    }
}
