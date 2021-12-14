using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seventh.DGuard.Database
{
    public class Server : BaseEntity
    {
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string Ip { get; set; }

        public int Port { get; set; }

        public virtual ICollection<Video> Videos { get; set; }
    }
}
