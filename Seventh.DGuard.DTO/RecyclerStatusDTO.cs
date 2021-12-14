using System;

namespace Seventh.DGuard.DTO
{
    public class RecyclerStatusDTO : BaseDTO
    {
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Days { get; set; }
    }

    public class RecyclerStatusDTO_In : RecyclerStatusDTO
    {
    }

    public class RecyclerStatusDTO_Out : RecyclerStatusDTO
    {
    }
    public class RecyclerReportDTO
    {
        public string Status { get; set; }
    }
}
