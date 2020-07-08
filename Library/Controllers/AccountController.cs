

using Library.Models;
using Library.ViewModels;
using Library.ViewRender;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace Library.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IViewRenderService _viewRenderService;
 
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IViewRenderService viewRenderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _viewRenderService = viewRenderService;
         
        }

        [HttpPost]
        public async Task<JsonResult> Register([FromBody] RegisterViewModel model)
        {
            bool registrationIsCorrect = false;

            if (ModelState.IsValid)
            {
                #region Добавляем в БД нового пользователя
                User user = new User { Email = model.Email, UserName = model.Email };
                var result  =    await  _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    registrationIsCorrect = true;
                    // установка куки
                    await _signInManager.SignInAsync(user, false);

                    return Json(new{registrationIsCorrect, string.Empty });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                #endregion
            }

            #region Список несоответствий
            List<String> allErrors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(error => error.ErrorMessage)
                .ToList();
            #endregion

            var ViewComponent = await _viewRenderService.RenderToStringAsync("Shared/Components/RegisterError/Default", allErrors);

            return Json(new {registrationIsCorrect, ViewComponent });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Login([FromBody]LoginViewModel model)
        {
            bool loginIsCorrect = false;

            if (ModelState.IsValid)
            {
                #region Авторизация
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                #endregion

                if (result.Succeeded)
                {
                    loginIsCorrect = true;
                    return Json(new { loginIsCorrect, string.Empty });
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }

            #region Список несоответствий
            var allErrors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(error =>error.ErrorMessage)
                .ToList();
            #endregion

            var ViewComponent   = await _viewRenderService.RenderToStringAsync("Shared/Components/LoginError/Default", allErrors);
            return Json(new { loginIsCorrect, ViewComponent });

        }
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }     
    }
}
