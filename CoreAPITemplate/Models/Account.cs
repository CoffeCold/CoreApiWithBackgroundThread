using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreAPI.Models
{
    public class Account
    {
        [Key]
        [MaxLength(30)]
        [MinLength(10)]
        [RegularExpression(@"^[0-9]{2}[A-Z]{2}[0-9]{6,26}", ErrorMessage = "Invalid IBAN : two number, two alpha and then numbers")]
        public string Iban { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }

        //navigation property
        public ICollection<Transaction> Transactions { get; set; }

    }
}
