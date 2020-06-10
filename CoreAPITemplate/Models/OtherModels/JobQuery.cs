using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreAPI.Models
{
 

    public class JobQuery
    {
        public Guid JobId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ExecutionDomain ExecutionDomain { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


        public Job Job { get; set; }
    }
}
