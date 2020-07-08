using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Components
{
    public class RegisterViewComponent : ViewComponent
    {

        /// <summary>
        /// Создаю модальное окно для регистрации
        /// </summary>
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
