// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using servis.Areas.Identity.Data;
using servis.Models;

namespace servis.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<servisUser> _signInManager;
        private readonly UserManager<servisUser> _userManager;
        private readonly IUserStore<servisUser> _userStore;
        private readonly IUserEmailStore<servisUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PsychologistDBContext _context;
        public RegisterModel(
            UserManager<servisUser> userManager,
            IUserStore<servisUser> userStore,
            SignInManager<servisUser> signInManager,
            ILogger<RegisterModel> logger,
           // IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            PsychologistDBContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }


        public string ReturnUrl { get; set; }

 
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            //[Required]
            //[DataType(DataType.Text)]
            //[Display(Name = "Account for")]
            public string TypeA { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"^\(?([0-9]{1})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Формат номера неправильный")]

            [Display(Name = "Телефон")]
            public string PhoneNumber { get; set; }
            [Required]
           
            [Display(Name = "Возраст")]
            public int Year { get; set; }
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                Client client = new Client();

                client.Name = Input.FirstName;
                client.LastName = Input.LastName;
                client.Year = Input.Year;
                client.Phone = Input.PhoneNumber;
                client.Email = Input.Email;
                //client.UserId = userId;
                _context.Add(client);
                await _context.SaveChangesAsync();
                var user = CreateUser();
                user.ModelID = client.ID;
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.Year = Input.Year;
                user.Phone = Input.PhoneNumber;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("admin"))
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                if (!await _roleManager.RoleExistsAsync("psych"))
                    await _roleManager.CreateAsync(new IdentityRole("psych"));
                if (!await _roleManager.RoleExistsAsync("guest"))
                    await _roleManager.CreateAsync(new IdentityRole("guest"));
                await _userManager.AddToRoleAsync(user, "guest");
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                  

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

        private servisUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<servisUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(servisUser)}'. " +
                    $"Ensure that '{nameof(servisUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<servisUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<servisUser>)_userStore;
        }
    }
}
