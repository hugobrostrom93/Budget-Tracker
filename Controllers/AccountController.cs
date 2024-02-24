using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using BudgetTrackerHugo.Models;

namespace BudgetTrackerHugo.Controllers;
public class AccountController : Controller
{
    // Mocked user data
    private const string MockedUsername = "demo";
    private const string MockedPassword = "pass"; 
    public IActionResult Login()
    {
        return View();
    }

    [Authorize]
    public IActionResult Logout()
    {
        return SignOut(
        new AuthenticationProperties
        {
            RedirectUri = Url.Action("Index", "Home")
        },
        CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [Authorize] 
    public IActionResult SecretInfo()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(LoginViewModel model)
    {
        // Check model validators
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Mocked user verification
        if (model.Username == MockedUsername && model.Password == MockedPassword)
        {
            // Set up the session/cookie for the authenticated user.
            var claims = new[] { new Claim(ClaimTypes.Name, model.Username) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            // Redirect to a secure area of your application.
            return RedirectToAction("Index", "Dashboard");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt."); // Generic error message for security reasons.
        return View(model);
    }

    // Comment out code below to enable Google Log in

    //public IActionResult GoogleLogin()
    //{
    //    var authProperties = new AuthenticationProperties
    //    {
    //        RedirectUri = Url.Action("GoogleLoginCallback", "Account")
    //    };
    //    return Challenge(authProperties, GoogleDefaults.AuthenticationScheme);
    //}

    //public async Task<IActionResult> GoogleLoginCallbackAsync()
    //{
    //    var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    //    if (!result.Succeeded)
    //    {
    //        // Handle failure: return to the login page, show an error, etc.
    //        return RedirectToAction("Home");
    //    }
    //    // Here, you could fetch information from result.Principal to store in your database,
    //    // or to find an existing user.
    //    return RedirectToAction("Index", "Dashboard");
    //}

    //public async Task GoogleLoginTwo()
    //{
    //    await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
    //    {
    //        RedirectUri = Url.Action(/*"GoogleLoginCallback", "Account"*/"GoogleResponse")
    //    });
    //}
    //public async Task<IActionResult> GoogleResponse()
    //{
    //    var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    //    var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
    //    {
    //        claim.Issuer,
    //        claim.OriginalIssuer,
    //        claim.Type,
    //        claim.Value
    //    });

    //    return RedirectToAction("Index", "Dashboard");
    //}
}
