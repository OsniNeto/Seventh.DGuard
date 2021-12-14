using Microsoft.Extensions.Logging;
using Seventh.DGuard.Database;
using Seventh.DGuard.Repository.Interface;

namespace Seventh.DGuard.Repository
{
    public class RecyclerStatusRepository : BaseRepository<RecyclerStatus>, IRecyclerStatusRepository
    {
        public RecyclerStatusRepository(SeventhDGuardContext context, ILogger<RecyclerStatus> logger) : base(context, logger) { }
    }
}