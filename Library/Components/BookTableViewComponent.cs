using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using Library.DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Html;

namespace Library.Components
{
    public class BookTableViewComponent : ViewComponent
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<User>  _userManager;

        public BookTableViewComponent(UserManager<User> userManager, ApplicationContext context)
        {
            _db = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

          User  user        = await _userManager.GetUserAsync(Request.HttpContext.User);
          var   books       = await _db.Books
                .Include(p=>p.BookUsers)
                .ThenInclude(u=>u.User)
                .ToListAsync();

            if (user != null) 
            {
                return View((books, user));
            }

            return new HtmlContentViewComponentResult(new HtmlString(""));


        }
    }
}
