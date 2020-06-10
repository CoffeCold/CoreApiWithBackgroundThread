using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreAPI.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid Target Price; Maximum Two Decimal Points.")]
        [Range(0, 9999999999999999.99)]
        public Decimal Amount { get; set; }

        [MaxLength(30)]
        [MinLength(10)]
        public string Counterparty { get; set; }

        //FK
        [MaxLength(30)]
        [MinLength(10)]
        [ForeignKey("Account")]
        public string AccountIban { get; set; }

        //navigation property
        public Account Account { get; set; }

    }
}

//using System.ComponentModel.DataAnnotations.Schema;

//public class Student
//{
//    public int StudentID { get; set; }
//    public string StudentName { get; set; }

//    //Foreign key for Standard
//    public int StandardId { get; set; }
//    public Standard Standard { get; set; }
//}

//public class Standard
//{
//    public int StandardId { get; set; }
//    public string StandardName { get; set; }

//    public ICollection<Student> Students { get; set; }
//}
