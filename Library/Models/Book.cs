using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid                 Id          { get; set; }
        [Required]
        public string               Name        { get; set; }
        [Required]
        public string               Author      { get; set; }

        public List<BookUser>       BookUsers   { get; set; }

        [NotMapped]
        public int NumberOfReaders 
        { 
            get 
            {
               return BookUsers?.Count ?? 0;
            } 
        }

        public Book() 
        {
            BookUsers = new List<BookUser>();
        }

    }
}
