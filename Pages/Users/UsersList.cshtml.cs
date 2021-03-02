using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using uul_api.Models;
using uul_web.Models.Clients;

namespace uul_web.Pages.Users
{
    public class UsersListModel : PageModel {
        private readonly UsersClient _client;
        private readonly ILogger<UsersListModel> _logger;
        public UsersListModel(UsersClient usersClient, ILogger<UsersListModel> logger) {
            _client = usersClient;
            _logger = logger;
        }

       
        public ICollection<UserWebInfoDTO> users = new List<UserWebInfoDTO>();

        public async Task<IActionResult> OnGetAsync() 
        {
            var response = await _client.GetUsersListAsync();

            if (response == null || !response.Success) {
                ModelState.AddModelError(string.Empty, "Failed to get users");
                return Page();
            }
            users = (ICollection<UserWebInfoDTO>)response.Data;
            return Page();
        }

        public IActionResult OnGetEdit(string Id) {
            return RedirectToPage("/Users/UserEdit", "UserProfile", new { passedObject = Id });
        }
    }
}
