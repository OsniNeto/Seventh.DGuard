using System;

namespace Seventh.DGuard.DTO.Filter
{
    public class VideoFilterDTO : BaseFilterDTO
    {
        public string Decription { get; set; }
        public Guid? ServerId { get; set; }
    }
}
