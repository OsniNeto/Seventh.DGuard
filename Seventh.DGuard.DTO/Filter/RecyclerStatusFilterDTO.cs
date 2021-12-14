using System;

namespace Seventh.DGuard.DTO.Filter
{
    public class RecyclerStatusFilterDTO : BaseFilterDTO
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Days { get; set; }
    }
}
