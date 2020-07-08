using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Components
{
    public class LoginErrorViewComponent : ViewComponent
    {


        public IViewComponentResult Invoke(List<String> ErrorList)
        {
            return View(ErrorList);
        }
    }
}
