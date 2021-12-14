using AutoMapper;
using EducSy.DataTransferObject;
using Seventh.DGuard.Business.Interface;
using Seventh.DGuard.Database;
using Seventh.DGuard.DTO;
using Seventh.DGuard.DTO.Filter;
using Seventh.DGuard.Repository.Interface;
using System;
using System.IO;

namespace Seventh.DGuard.Business
{
    public class VideoBO : BaseBO<Video, VideoDTO_In, VideoDTO_Out, VideoFilterDTO, IVideoRepository>, IVideoBO
    {
        public VideoBO(IVideoRepository repository, IMapper mapper) : base(repository, mapper) { }

        public override ResultDTO<VideoDTO_Out> Validate_Add(VideoDTO_In model)
        {
            var baseResponse = base.Validate_Add(model);
            if (!baseResponse.Success)
                return baseResponse;

            return ResultFactory.GenerateResponse<VideoDTO_Out>(true);
        }

        public ResultListDTO<VideoDTO_Out> FindByServer(Guid serverId)
        {
            return base.Find(new VideoFilterDTO { ServerId = serverId });
        }

        public ResultListDTO<VideoDTO_Out> FindToRemove(int days)
        {
            var minDate = DateTime.Now.AddDays(-days);
            var videos = ModelOutFromEntity(_repository.Find(x => x.AddDate < minDate));
            return ResultFactory.GenerateResponseList(videos);
        }

        public ResultDTO<VideoDTO_Out> Add(Guid serverId, VideoDTO_In t)
        {
            var fileName = t.Filename;
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{Guid.NewGuid()}.{TryGetFileType(t.File)}";
                t.Filename = fileName;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            File.WriteAllBytes(Path.Combine(filePath, fileName), Convert.FromBase64String(t.File));
            t.ServerId = serverId;
            return base.Add(t);
        }

        public DownloadVideoDTO Download(Guid videoId)
        {
            var video = Get(videoId);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images");
            var fileBytes = File.ReadAllBytes(Path.Combine(filePath, video.Return.Filename));

            return new DownloadVideoDTO { FileBytes = fileBytes, FileName = video.Return.Filename };
        }

        private string TryGetFileType(string file)
        {
            var data = file.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }
    }
}
