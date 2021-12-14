using Microsoft.Extensions.Logging;
using Seventh.DGuard.Database;
using Seventh.DGuard.Repository.Interface;

namespace Seventh.DGuard.Repository
{
    public class ServerRepository : BaseRepository<Server>, IServerRepository
    {
        public ServerRepository(SeventhDGuardContext context, ILogger<Server> logger) : base(context, logger) { }
    }
}