using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Member
    {
        [Key]
        public int Id{ get; set; }
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }
        [Required]
        public DateTime MembershipExpireDate { get; set; }
        [Required]
        [MaxLength(16)]
        public string PhoneNumber { get; set; }

        public ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }
}
