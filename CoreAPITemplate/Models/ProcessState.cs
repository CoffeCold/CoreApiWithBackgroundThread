using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Models
{
    public class ProcessState
    {
        public int Id { get; set; }
        public string State { get; set; }
        public DateTime Logdate { get; set; }
    }
}
