using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Models
{
    public class ProcessLog
    {
        public int Id { get; set; }
        public string Logcomment { get; set; }
        public DateTime Logdate { get; set; }
    }
}
