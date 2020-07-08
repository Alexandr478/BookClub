using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModels
{  
        public class LoginViewModel
        {
            [Required(ErrorMessage = "Не указан Email пользователя")]
            [DataType(DataType.EmailAddress)]   
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Не указан пароль пользователя")]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [Display(Name = "Запомнить?")]
            public bool RememberMe { get; set; }

            public string ReturnUrl { get; set; }
        }  
}
