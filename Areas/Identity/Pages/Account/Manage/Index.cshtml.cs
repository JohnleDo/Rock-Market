using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rock_Market.Areas.Identity.Data;

namespace Rock_Market.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Rock_MarketUser> _userManager;
        private readonly SignInManager<Rock_MarketUser> _signInManager;
        private readonly IHostingEnvironment _hostEnvironment;

        public IndexModel(
            UserManager<Rock_MarketUser> userManager,
            SignInManager<Rock_MarketUser> signInManager,
             IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostEnvironment = hostingEnvironment;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Home Address")]
            public string Address { get; set; }

            [Display(Name = "City")]
            public string City { get; set; }

            [Display(Name = "State")]
            public string State { get; set; }

            [Display(Name = "Profile Image")]
            public IFormFile ProfileImage { get; set; }
        }

        private async Task LoadAsync(Rock_MarketUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                City = user.City,
                State = user.State
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Updating User's Phone number or setting one if not done so.
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.ProfileImage != null)
            {
                // Copying File into our FileSystem
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.ProfileImage.FileName;
                string path = Path.Combine(wwwRootPath + "/images/User Profile Images/", uniqueFileName);
                Input.ProfileImage.CopyTo(new FileStream(path, FileMode.Create));

                FileInfo file = new FileInfo(_hostEnvironment.WebRootPath + "/images/User Profile Images/" + user.ProfilePath);
                if (file.Exists)
                {
                    // Need Garbage Collection to clear up process for a second delection whenever user changes pictures more than once in a session
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    file.Delete();
                }
                user.ProfilePath = uniqueFileName;
            }

            // Updating User information.
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.Address = Input.Address;
            user.City = Input.City;
            user.State = Input.State;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to update information.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public string SaveFile(Rock_MarketUser user)
        {
            return "1";
        }
    }
}
