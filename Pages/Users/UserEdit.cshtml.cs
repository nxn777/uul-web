using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using uul_api.Models;

namespace uul_web.Pages.Users
{
    public class UserEditModel : PageModel
    {
        public UserWebInfoDTO UserProfile { get; set; }
        public IActionResult OnGetUserProfile(string passedObject) {
            if (passedObject == null) { return NotFound(); }
            UserProfile  = JsonConvert.DeserializeObject<UserWebInfoDTO>(passedObject);

            if (UserProfile == null) {
                return NotFound();
            }

            return Page();
        }
    }
}
