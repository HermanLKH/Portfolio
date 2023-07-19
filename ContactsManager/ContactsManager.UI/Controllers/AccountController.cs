using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using CRUDExample.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace ContactsManager.UI.Controllers
{
	[Route("[controller]/[action]")]
	// allow access without login (bypass authorization policy)
	//[AllowAnonymous]
	public class AccountController : Controller
	{
		// used to call business logic methods to manage users
		private readonly UserManager<ApplicationUser> _userManager;
		// used to handle sign in activities of users 
		private readonly SignInManager<ApplicationUser> _signInManager;
		// used to perform all operations to manage user roles
		private readonly RoleManager<ApplicationRole> _roleManager;

		// inject identity managers to use identity services
		public AccountController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			RoleManager<ApplicationRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
        }

		[HttpGet]
		[Authorize("NotAuthorized")]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
        [Authorize("NotAuthorized")]
		//[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
		{
			// check for validation error
			if(!ModelState.IsValid)
			{
				ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors)
					.Select(temp => temp.ErrorMessage);
				return View(registerDTO);
			}

			ApplicationUser user = new ApplicationUser() 
			{
				Email = registerDTO.Email,
				PhoneNumber = registerDTO.Phone,
				UserName = registerDTO.Email,
				Name = registerDTO.Name,
			};

			// business logic method to create user
			// identity result => represent result of execution of identity operation
			IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

			if(result.Succeeded)
			{
				if(registerDTO.UserType == UserTypeOptions.Admin)
				{
					// create admin role
					if(await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
					{
						ApplicationRole role = new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() };

						await _roleManager.CreateAsync(role);
					}
                    // add the new user to the user role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
				}
				else
				{
                    // create user role
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
                    {
                        ApplicationRole role = new ApplicationRole() { Name = UserTypeOptions.User.ToString() };

                        await _roleManager.CreateAsync(role);
                    }
					// add the new user to the user role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                }
				await _signInManager.SignInAsync(user, isPersistent: false);

				return RedirectToAction(nameof(PersonsController.Index), "Persons");
			}
			else
			{
                foreach (IdentityError error in result.Errors)
                {
					ModelState.AddModelError("Register", error.Description);
                }
				return View(registerDTO);
            }
		}

		[HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? returnUrl) 
		{
			if(ModelState.IsValid)
			{
                var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, 
					isPersistent: true, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					ApplicationUser user = await _userManager.FindByEmailAsync(loginDTO.Email);

                    if (user != null)
					{
						// admin
						if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
						{
							return RedirectToAction("Index", "Home", new {area = "Admin"});
						}
					}
					// normal user
					// localurl => local domain
					if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
					return RedirectToAction(nameof(PersonsController.Index), "Persons");
				}
				else
				{
					ModelState.AddModelError("Login", "Invalid email or password");
					return View(loginDTO);
				}
			}
			ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors)
				.Select(temp => temp.ErrorMessage);
			return View(loginDTO);
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction(nameof(PersonsController.Index), "Persons");
		}

		[AllowAnonymous]
		public async Task<IActionResult> IsEmailRegistered(string email)
		{
			ApplicationUser user = await _userManager.FindByEmailAsync(email);

			if(user == null)
			{
				return Json(true);
			}
			else
			{
				return Json(false);
			}
		}
	}
}
