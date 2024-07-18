using System.Security.Claims;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

namespace IdentityService.Pages.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [BindProperty]
        public RegisterViewModel Input {get; set;}
        [BindProperty]
        public bool RegisterSuccess { get; set;}
        public IActionResult OnGet(string returnUrl)
        {
            Input = new RegisterViewModel
            {
                ReturnUrl = returnUrl,
            };
            return Page();
        }
        public async Task<IActionResult> onPost()
        {
            if(Input.Button != "register") return Redirect("~/");
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var claim = new Claim(JwtClaimTypes.Name, Input.FullName);
                    var claimResult = await _userManager.AddClaimAsync(user, claim);

                    if (claimResult.Succeeded)
                    {
                        RegisterSuccess = true;
                    }
                }
            }
            return Page();
        }
    }
}
