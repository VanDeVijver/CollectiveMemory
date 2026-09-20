
using CollectiveMemory.Core.Services.Interfaces;

using CollectiveMemory.Core.Entities;
using CollectiveMemory.Core.Data;
using Microsoft.Extensions.Logging;

namespace CollectiveMemory.Core.Services
{
    public class MemberService : BaseService<Member, int>, IMemberService
    {
        public MemberService(
            ApplicationDbContext context, 
            ILogger<BaseService<Member, int>> logger) : base(context, logger)
        { }
    }
}
