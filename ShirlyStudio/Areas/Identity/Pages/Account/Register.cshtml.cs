using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebApplication4.Models;
using System.Text;

namespace ShirlyStudio.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public RoleManager<IdentityRole> RoleManager { get; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "אימייל")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "סיסמא")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "אימות סיסמא")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
			
		    [Required]
            [DataType(DataType.Text)]
            [Display(Name = "שם מלא")]
            public string Name { get; set; }
			
	        [Required]

            [Display(Name = "גיל")]
            public int Age { get; set; }
			
			 [Required(ErrorMessage = "מספר הטלפון אינו חוקי")]
        [Display(Name = "פלאפון")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "מספר הטלפון אינו חוקי")]
        public string PhoneNumber { get; set; }
			
			
        }       

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
				Customer cas = new Customer { Email = Input.Email, CustomerName = Input.Name, Age = Input.Age,PhoneNumber = Input.PhoneNumber};
            
				string userJson = JsonConvert.SerializeObject(cas);
				returnUrl = returnUrl ?? Url.Content("~/Customers/CreateFromUser/?userjson=" + System.Net.WebUtility.UrlEncode(userJson));//user
				var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    string[] roles = { "Admin", "Customer" };

                    foreach (var role in roles)
                    {

                        if (!await _roleManager.RoleExistsAsync(role))
                        {
                            var users = new IdentityRole(role);
                            await _roleManager.CreateAsync(users);

                        }
                    }

                    var poweruser = new IdentityUser
                    {
                        UserName = "admin@gmail.com",
                        Email = "admin@gmail.com",
                    };
                    string userPWD = "Q1w2e3!";
                    var _user = await _userManager.FindByEmailAsync("admin@gmail.com");

                    if (_user == null)
                    {
                        var createPowerUser = await _userManager.CreateAsync(poweruser, userPWD);
                        if (createPowerUser.Succeeded)
                        {
                            //here we tie the new user to the role
                            await _userManager.AddToRoleAsync(poweruser, "Admin");

                        }
                    }
                    await _userManager.AddToRoleAsync(user, "Customer");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
      
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
