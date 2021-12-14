using EducSy.DataTransferObject;
using Seventh.DGuard.Database;
using Seventh.DGuard.DTO;
using Seventh.DGuard.DTO.Filter;
using System;

namespace Seventh.DGuard.Business.Interface
{
    public interface IVideoBO : IBaseBO<Video, VideoDTO_In, VideoDTO_Out, VideoFilterDTO>
    {
        ResultListDTO<VideoDTO_Out> FindByServer(Guid serverId);
        ResultDTO<VideoDTO_Out> Add(Guid serverId, VideoDTO_In t);
        DownloadVideoDTO Download(Guid videoId);
        ResultListDTO<VideoDTO_Out> FindToRemove(int days);
    }
}