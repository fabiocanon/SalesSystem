using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesSystem.Areas.Users.Models;

namespace SalesSystem.Areas.Users.Pages.Account
{
    public class RegisterModel : PageModel
    {
        public void OnGet()
        {
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel: InputModelRegister
        {
            public IFormFile AvatarImage { get; set; }
            [TempData]
            public string ErrorMessage{ get; set; }
        }
    }
}
