using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreAPI.Models
{
    //[JsonConverter(typeof(StringEnumConverter))]
    public enum ExecutionDomain { Bulk, Batch}

    public class JobSettings
    {
        [Key]
        public Guid JobId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ExecutionDomain ExecutionDomain { get; set; }
        public string JobProperty1 { get; set; }

        public string Description { get; set; }

        public ICollection<JobLog> Logs { get; set; }
        public ICollection<JobState> States { get; set; }
    }
}
