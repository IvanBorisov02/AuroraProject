using System.Threading.Tasks;
using AS.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AS.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ASUser> _signInManager;

        public LogoutModel(SignInManager<ASUser> signInManager)
        {
            _signInManager = signInManager;

        }

        public async Task<IActionResult> OnPost()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }
    }
}
