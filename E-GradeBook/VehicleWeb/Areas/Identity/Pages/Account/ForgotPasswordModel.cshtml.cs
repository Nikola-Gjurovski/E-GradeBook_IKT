using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Services.Interface;
using System.Text;
using System.Text.Encodings.Web;

public class ForgotPasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICustomEmailSender _emailSender;

    public ForgotPasswordModel(UserManager<ApplicationUser> userManager, ICustomEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    [BindProperty]
    public string Email { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || string.IsNullOrEmpty(Email))
            return Page();

        var user = await _userManager.FindByEmailAsync(Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            return RedirectToPage("./ForgotPasswordConfirmation");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        // ✅ FIX: Encode the token safely
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var callbackUrl = Url.Page(
            "/Account/ResetPassword",
            pageHandler: null,
            values: new { area = "Identity", userId = user.Id, code = encodedToken },
            protocol: Request.Scheme);

        await _emailSender.SendEmailAsync(
            Email,
            "Reset Password",
            $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        return RedirectToPage("./ForgotPasswordConfirmation");
    }
}
