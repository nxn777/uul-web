using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace uul_web.Pages.SignOut {
    [AllowAnonymous]
    public class SignedOutModel : PageModel {
        public IActionResult OnGet() {
            if (User.Identity.IsAuthenticated) {
                // Redirect to home page if the user is authenticated.
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}
