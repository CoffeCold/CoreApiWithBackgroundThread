using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreAPI.Models
{
    public class JobLog
    {
        [Key]
        public Guid LogId { get; set; }
        public string Logcomment { get; set; }
        public DateTime Logdate { get; set; }

        //Foreign key for Job
        [ForeignKey("JobSettings")]
        public Guid JobId { get; set; }
        public Job Job { get; set; }
    }
}
