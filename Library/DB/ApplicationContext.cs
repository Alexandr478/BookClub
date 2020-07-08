
using Library.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.DB
{
    public class ApplicationContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Book>     Books      { get; set; }
        public DbSet<BookUser> BookUsers  { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) 
            : base(options)
        {
            Database.EnsureCreated(); 
        }
   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Настройка промежуточной таблицы 'BookUser' для таблиц 'Book' и 'User'
            modelBuilder.Entity<BookUser>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.BookUsers)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<BookUser>()
                .HasOne(sc => sc.Book)
                .WithMany(c => c.BookUsers)
                .HasForeignKey(sc => sc.BookId);
            #endregion

            #region Инициализация таблицы начальными данными
            modelBuilder.Entity<Book>().HasData(
                new Book[] { 

                    new Book {Id = Guid.NewGuid(),Name = "Мастер и Маргарита", Author = "Михаил Булгаков"  },
                    new Book {Id = Guid.NewGuid(),Name = "Три товарища", Author = "Эрих Мария Ремарк" },
                    new Book {Id = Guid.NewGuid(),Name = "Цветы для Элджернона", Author = "Даниел Киз" },
                    new Book {Id = Guid.NewGuid(),Name = "Над пропастью во ржи", Author = "Джером Д. Сэлинджер" },
                    new Book {Id = Guid.NewGuid(),Name = "Маленький принц", Author = "Антуан де Сент-Экзюпери" },
                    new Book {Id = Guid.NewGuid(),Name = "Портрет Дориана Грея", Author = "Оскар Уайльд" },
                    new Book {Id = Guid.NewGuid(),Name = "Вино из одуванчиков", Author = "Рей Брэдбери" },
                    new Book {Id = Guid.NewGuid(),Name = "Убить пересмешника", Author = "Харпер Ли" },
                    new Book {Id = Guid.NewGuid(),Name = "Атлант расправил плечи", Author = "Айн Рэнд" },
                    new Book {Id = Guid.NewGuid(),Name = "Сто лет одиночества", Author = "Габриэль Гарсиа Маркес" },
                    new Book {Id = Guid.NewGuid(),Name = "Анна Каренина", Author = "Лев Толстой" },
                    new Book {Id = Guid.NewGuid(),Name = "Преступление и наказание", Author = "Фёдор Достоевский" },
                    new Book {Id = Guid.NewGuid(),Name = "Двенадцать стульев", Author = "Евгений Петров и др." },
                    new Book {Id = Guid.NewGuid(),Name = "Граф Монте-Кристо", Author = "Александр Дюма" },
                    new Book {Id = Guid.NewGuid(),Name = "Евгений Онегин", Author = "Александр Пушкин" },
                });
            #endregion
        }
    }


}
