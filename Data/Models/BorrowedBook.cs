using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class BorrowedBook
    {
        [Key, Column(Order = 0)]
        public int BookID { get; set; }
        [Key, Column(Order = 1)]
        public int MemberID { get; set; }
        [Required]
        public DateTime BorrowDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        [ForeignKey("BookID")]
        public Book? Book { get; set; }

        [ForeignKey("MemberID")]
        public Member? Member { get; set; }
    }
}
