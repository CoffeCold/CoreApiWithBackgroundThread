using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreAPI.Models
{
    public class JobState
    {
        [Key]
        public Guid StateID { get; set; }
        public string State { get; set; }
        public DateTime Logdate { get; set; }

        //Foreign key for Job
        [ForeignKey("JobSettings")]
        public Guid JobId { get; set; }
        public JobSettings Job { get; set; }
    }
}
