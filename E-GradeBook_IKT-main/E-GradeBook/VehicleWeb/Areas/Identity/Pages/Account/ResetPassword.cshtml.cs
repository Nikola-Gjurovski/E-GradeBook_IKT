//using Domain.Identity;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.ComponentModel.DataAnnotations;
//using System.Text;
//using Microsoft.AspNetCore.WebUtilities;

//public class ResetPasswordModel : PageModel
//{
//    private readonly UserManager<ApplicationUser> _userManager;

//    public ResetPasswordModel(UserManager<ApplicationUser> userManager)
//    {
//        _userManager = userManager;
//    }

//    [BindProperty]
//    public InputModel Input { get; set; }

//    public class InputModel
//    {
//        [Required]
//        public string UserId { get; set; }

//        [Required]
//        public string Code { get; set; }

//        [Required(ErrorMessage = "Внесете е-пошта.")]
//        [EmailAddress(ErrorMessage = "Внесете валидна е-пошта.")]
//        public string Email { get; set; }

//        [Required(ErrorMessage = "Внесете лозинка.")]
//        [StringLength(100, ErrorMessage = "{0} мора да биде најмалку {2} и најмногу {1} карактери.", MinimumLength = 6)]
//        [DataType(DataType.Password)]
//        public string Password { get; set; }

//        [DataType(DataType.Password)]
//        [Display(Name = "Потврди лозинка")]
//        [Compare("Password", ErrorMessage = "Лозинките не се совпаѓаат.")]
//        public string ConfirmPassword { get; set; }
//    }

//    public IActionResult OnGet(string code = null, string userId = null)
//    {
//        if (code == null || userId == null)
//        {
//            ModelState.AddModelError(string.Empty, "Недостасува код или корисник.");
//            return Page();
//        }

//        Input = new InputModel
//        {
//            Code = code,
//            UserId = userId
//        };

//        return Page();
//    }

//    public async Task<IActionResult> OnPostAsync()
//    {
//        if (!ModelState.IsValid)
//        {
//            ModelState.AddModelError(string.Empty, "Внесете ги сите полиња правилно.");
//            return Page();
//        }

//        var user = await _userManager.FindByIdAsync(Input.UserId);
//        if (user == null || user.Email.ToLower() != Input.Email.ToLower())
//        {
//            // ✅ Do not reveal if user exists for security
//            return RedirectToPage("./ResetPasswordConfirmation");
//        }

//        // ✅ Decode token safely
//        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Input.Code));

//        var result = await _userManager.ResetPasswordAsync(user, decodedToken, Input.Password);
//        if (result.Succeeded)
//        {
//            return RedirectToPage("./ResetPasswordConfirmation");
//        }

//        foreach (var error in result.Errors)
//        {
//            ModelState.AddModelError(string.Empty, error.Description);
//        }

//        return Page();
//    }
//}
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

public class ResetPasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ResetPasswordModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} and at most {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public IActionResult OnGet(string code = null, string userId = null)
    {
        if (code == null || userId == null)
        {
            ModelState.AddModelError(string.Empty, "A code and user must be supplied.");
            return Page();
        }

        Input = new InputModel
        {
            Code = code,
            UserId = userId
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please fill in all fields correctly.");
            return Page();
        }

        var user = await _userManager.FindByIdAsync(Input.UserId);
        if (user == null || user.Email.ToLower() != Input.Email.ToLower())
        {
            // ✅ Do not reveal if user exists
            return RedirectToPage("./ResetPasswordConfirmation");
        }

        // ✅ Decode token safely
        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Input.Code));

        var result = await _userManager.ResetPasswordAsync(user, decodedToken, Input.Password);
        if (result.Succeeded)
        {
            return RedirectToPage("./ResetPasswordConfirmation");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }
}
