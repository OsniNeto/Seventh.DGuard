using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seventh.DGuard.Database
{
    public class RecyclerStatus : BaseEntity
    {
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Days { get; set; }
    }
}
