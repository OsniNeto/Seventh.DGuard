using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seventh.DGuard.Database
{
    public class Video : BaseEntity
    {
        [Column(TypeName = "varchar(255)")]
        public string Description { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Filename { get; set; }

        public DateTime AddDate { get; set; }

        [ForeignKey("Server")]
        public Guid ServerId { get; set; }
        public Server Server { get; set; }
    }
}
