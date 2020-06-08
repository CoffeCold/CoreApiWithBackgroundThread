using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using CoreAPI.Models;
using CoreAPI.Helpers;

namespace CoreAPI.Services
{

    public interface IBatchService : IGenericService
    {
        Task<TaskSetting> StartBatch(TaskSetting taskSettings);
    

    }
    public interface IBulkService : IGenericService
    {
        Task<TaskSetting> StartBulk(TaskSetting taskSettings);
   

    }
    public interface IGenericService
    {
        Task<ProcessState> Check(TaskSetting taskSettings);
        Task<IEnumerable<ProcessLog>> GetLogs(TaskSetting taskSettings);
    }

}
