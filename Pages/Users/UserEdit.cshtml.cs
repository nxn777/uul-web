using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using uul_api.Models;
using uul_web.Models.Clients;

namespace uul_web.Pages.Users
{
    public class UserEditModel : PageModel
    {
        [BindProperty]
        public UserWebInfoDTO UserProfile { get; set; }
        private readonly UsersClient _client;
        [TempData]
        public string InitialObject { get; set; }
        public UserEditModel(UsersClient usersClient) {
            _client = usersClient;
        }

        public IActionResult OnGetUserProfile(string passedObject) {
            if (passedObject == null) { return NotFound(); }
            UserProfile  = JsonConvert.DeserializeObject<UserWebInfoDTO>(passedObject);

            if (UserProfile == null) {
                return NotFound();
            }
            InitialObject = passedObject;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            var profile = JsonConvert.DeserializeObject<UserWebInfoDTO>(InitialObject);
            profile.IsActivated = UserProfile.IsActivated;
            var response = await _client.UpdateUsertAsync(profile);

            if (response == null || !response.Success) {
                ModelState.AddModelError(string.Empty, "Failed to update the user");
                return Page();
            }

            return RedirectToPage("/Users/UsersList");
        }
    }
}
