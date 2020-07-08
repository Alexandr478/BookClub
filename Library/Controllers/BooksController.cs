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

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<User>  _userManager;

        public BooksController(UserManager<User> userManager,ApplicationContext context)
        {
            _db          = context;
            _userManager = userManager;
        }


        [HttpGet] 
        public async Task<ActionResult> Index()
        {
            User        user    = null;
            List<Book>  books   = null;

            #region Проверяю авторизацию
            if (!User.Identity.IsAuthenticated)
            {
               return RedirectToAction("Index", "Home");
            }
            #endregion

            #region Текущий пользователь
            user = await _userManager.GetUserAsync(Request.HttpContext.User);
            #endregion

            #region Загружаю книги которые пользователь указал как прочитанные
            try
            {
                books = await _db.Books
               .Include(p => p.BookUsers)
               .ThenInclude(u => u.User)
               .Where(p => p.BookUsers.Any(a => a.UserId == user.Id)).ToListAsync();
               
                if (books == null)
                {
                    throw new Exception();
                }
            }
            catch
            {
                
            }


            #endregion

            return View(books);
        }

        //обертка
        public class GuidWrapper 
        { 
            public String Value { get; set; }
        }

        [HttpPost]
        public async Task<JsonResult> AddBookToReadList([FromBody] GuidWrapper data)
        { 
            Guid    bookId              = new Guid(); 
            Book    currBook            = null;
            User    user                = null;

            String  message             = String.Empty; //Сообщение для пользователя
            bool    changeButtonStatus  = true;  //Нужно ли изменить вид кнопки?

            #region Проверяю авторизацию
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index", "Home");
            }
            #endregion

            #region Проверяю корректность запроса
            if (!Guid.TryParse(data.Value, out bookId)) 
            {
                return Json(new { bookId, changeButtonStatus = false, Message = "Ошибка запроса" });
            }
            #endregion

            #region Текущий пользователь
            user = await _userManager.GetUserAsync(Request.HttpContext.User);
            #endregion

            #region Загружаю книгу которую пользователь указал как прочитанную
            
            try
            {
               currBook = await _db.Books
              .Include(p => p.BookUsers)
              .ThenInclude(u => u.User)
              .Where(b => b.Id == bookId)
              .FirstOrDefaultAsync();

                if (currBook == null) 
                {
                    throw new Exception();
                }
            }
            catch
            {
                return Json(new { bookId, changeButtonStatus = false, message = "Не удалось сохранить выбор пользователя !" });
            }
          

            #endregion

            #region Проверяю м.б. книга уже указана как прочитанная
            if (currBook.BookUsers.Any(u => u.UserId == user.Id))
            {
                return Json(new { bookId, changeButtonStatus, userReadCount = currBook.BookUsers.Count(), Message = $"Книга '{currBook.Name}' уже была выбрана вами как прочитанная !!!" });
            }
            #endregion

            #region Связываю профиль пользователя с книгой
            try
            {
                await _db.BookUsers.AddAsync(new BookUser { UserId = user.Id, BookId = bookId });
                await _db.SaveChangesAsync();
                message = $"Книга '{currBook.Name}' успешно помечена как 'Прочитанная'";
            }
            catch
            {
                changeButtonStatus = false;
                message            = "Не удалось сохранить выбор пользователя !";
            }
            #endregion

            return Json(new { bookId, userReadCount = currBook.BookUsers.Count(), changeButtonStatus, message });
        }

        [HttpPost]
        public async Task<JsonResult> DelBookFromReadList([FromBody] GuidWrapper data)
        {
            Guid bookId     = new Guid();
            Book currBook   = null;
            User user       = null;

            String message  = String.Empty; //Сообщение для пользователя
            bool   delRow   = true;  //Нужно ли удалить строку

            #region Проверяю авторизацию
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index", "Home");
            }
            #endregion

            #region Проверяю корректность запроса
            if (!Guid.TryParse(data.Value, out bookId))
            {
                return Json(new { bookId, delRow = false, Message = "Ошибка запроса" });
            }
            #endregion

            #region Текущий пользователь
            user = await _userManager.GetUserAsync(Request.HttpContext.User);
            #endregion

            #region Загружаю книгу которую пользователь хочет убрать из списка прочитанных

            try
            {
                currBook = await _db.Books
               .Where(b => b.Id == bookId)
               .FirstOrDefaultAsync();

                if (currBook == null)
                {
                    throw new Exception();
                }
            }
            catch
            {
                return Json(new { bookId, delRow = false, message = "Не удалось сохранить выбор пользователя !" });
            }


            #endregion  

            #region Удаляю запись в БД связывающую книгу и пользователя
            try
            {
                var bookUsers = await _db.BookUsers
                    .Where(p => (p.UserId == user.Id && p.BookId == bookId))
                    .FirstOrDefaultAsync();

                      _db.BookUsers.Remove(bookUsers);
                await _db.SaveChangesAsync();
                message = $"Книга '{currBook.Name}' успешно удалена из списка прочитанных";
            }
            catch
            {
                delRow = false;
                message = "Не удалось сохранить выбор пользователя !";
            }
            #endregion

            return Json(new { bookId, delRow, message });
        }


    }
}
