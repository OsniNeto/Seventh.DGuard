using Microsoft.Extensions.Logging;
using Seventh.DGuard.Database;
using Seventh.DGuard.Repository.Interface;

namespace Seventh.DGuard.Repository
{
    public class VideoRepository : BaseRepository<Video>, IVideoRepository
    {
        public VideoRepository(SeventhDGuardContext context, ILogger<Video> logger) : base(context, logger) { }
    }
}