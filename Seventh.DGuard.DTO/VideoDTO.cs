using Newtonsoft.Json;
using System;

namespace Seventh.DGuard.DTO
{
    public class VideoDTO : BaseDTO
    {
        public string Description { get; set; }
        public string Filename { get; set; }
        [JsonIgnore]
        public Guid ServerId { get; set; }
    }

    public class VideoDTO_In : VideoDTO
    {
        public string File { get; set; }
    }

    public class VideoDTO_Out : VideoDTO
    {
    }
    public class DownloadVideoDTO
    {
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
    }
}
