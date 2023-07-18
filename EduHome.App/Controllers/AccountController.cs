﻿using EduHome.App.Context;
using EduHome.App.Services.Interfaces;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace EduHome.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _mailService;

        public AccountController(EduHomeDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IEmailService mailService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }
        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterVM registerVM)
        {
            if(!ModelState.IsValid)
            {
                return View(registerVM);
            }
            AppUser appUser = new AppUser()
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                Email = registerVM.Email,
                UserName = registerVM.UserName,
            };
            var result =  await _userManager.CreateAsync(appUser, registerVM.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View(registerVM);
                }
            }
            await _userManager.AddToRoleAsync(appUser, "User");
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            string? link = Url.Action(action: "VerifyEmail", controller: "Account", values: new
            {
                token = token,
                mail = appUser.Email
            }, protocol: Request.Scheme);

            await _mailService.SendMail("nicatsoltanli03@gmail.com", appUser.Email,
                "Verify Email", "Click me to verify email", link, appUser.Name + " " + appUser.Surname);

            TempData["Register"] = "Please verify your email";
            return RedirectToAction("index","home");
        }
        public async Task<IActionResult> VerifyEmail(string token, string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            if(user is null)
            {
                return NotFound();
            }
            await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, true);
            TempData["Verify"] = "Succesfully SignUp";
            return RedirectToAction("index","home");
        }
        [Authorize]
        public async Task<IActionResult> Info()
        {
            string username = User.Identity.Name;
            AppUser appUser = await _userManager.FindByNameAsync(username);
            if (!await _userManager.IsInRoleAsync(appUser, "User"))
            {
                TempData["AdminInfo"] = "You cannot see Admin Info";
                return RedirectToAction("index","home");
            }
            return View(appUser);
            
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            AppUser appUser = await _userManager.FindByNameAsync(loginVM.UserName);
            if (appUser is null)
            {
                ModelState.AddModelError("", "Username or password is not correct ");
                return View(loginVM);
            }
            if (!await _userManager.IsInRoleAsync(appUser, "User"))
            {
                ModelState.AddModelError("", "Access Failed");
                return View(loginVM);
            }
            var result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Your account blocked 5 minutes");
                    return View(loginVM);

                }
                ModelState.AddModelError("", "Username or password is not correct ");
                return View(loginVM);
            }
            return RedirectToAction("index","home");
        }
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index","home");
        }
        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string mail)
        {
            if(mail is null)
            {
                ModelState.AddModelError("", "Please enter email");
                    return View();
            }
            var user = await _userManager.FindByEmailAsync(mail);
            if (user is null)
            {
                return NotFound();
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string? link = Url.Action(action: "ResetPassword", controller: "Account", values: new
            {
                token = token,
                mail = mail
            }, protocol: Request.Scheme);

            await _mailService.SendMail("nicatsoltanli03@gmail.com", user.Email,
            "Reset Password", "Click me for reseting password", link, user.Name + " " + user.Surname);
            return RedirectToAction("index", "home");
        }
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string mail,string token)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            if(user is null)
            {
                return NotFound();
            }
            ResetPasswordVM resetPasswordVM = new ResetPasswordVM()
            {
                Mail = mail,
                Token = token
            };
            return View(resetPasswordVM);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordVM.Mail);
            if (user is null)
            {
                return NotFound();
            }
            var result =  await _userManager.
                ResetPasswordAsync(user, resetPasswordVM.Token,resetPasswordVM.Password);
            if (!result.Succeeded)
            {
                foreach(var error in  result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(resetPasswordVM);
            }
            return RedirectToAction("login", "account");
        }

        [Authorize]
        public async Task<IActionResult> Update()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return NotFound();
            }
            UpdatedUserVM updatedUserVM = new UpdatedUserVM()
            {
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
            };
            return View(updatedUserVM);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(UpdatedUserVM model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (user is null)
            {
                return NotFound();
            }
            user.Name = model.Name;
            user.Email = model.Email;
            user.Surname = model.Surname;
            user.UserName = model.UserName;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(model);
                }

            }
            await _signInManager.SignInAsync(user, true);

            return RedirectToAction(nameof(Info));
        }
    }
}
