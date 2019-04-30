using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.Entities
{
    [Table("ip")]
    public class CIp : Entity<int>
    {
        //[IP] NVARCHAR(50) NULL,
        [MaxLength(50)]
        public virtual string IP { get; set; }
    }
}
