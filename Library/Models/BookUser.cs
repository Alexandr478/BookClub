using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class BookUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid     Id          { get; set; }

        public Guid     UserId      { get; set; }
        public User     User        { get; set; }

        public Guid     BookId      { get; set; }
        public Book     Book        { get; set; }
    }
}
