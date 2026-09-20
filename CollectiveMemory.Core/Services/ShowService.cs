using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectiveMemory.Core.Services.Interfaces;
using CollectiveMemory.Core.Entities;
using CollectiveMemory.Core.Data;
using Microsoft.Extensions.Logging;

namespace CollectiveMemory.Core.Services
{
    public class ShowService : BaseService<Show, int>, IShowService
    {
        public ShowService(ApplicationDbContext context, 
            ILogger<BaseService<Show, int>> logger) : base(context, logger)
        {}
    }
}
