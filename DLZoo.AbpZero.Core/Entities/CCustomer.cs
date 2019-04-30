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
    [Table("customers")]
    public class CCustomer:Entity<int>
    {
        //[customerName] NVARCHAR(50) NULL,
        [MaxLength(50)]
        public virtual string customerName { get; set; }
        //[address]      NVARCHAR(50) NULL,
        [MaxLength(50)]
        public virtual string address { get; set; }
        //[tel]          NVARCHAR(50) NULL,
        [MaxLength(50)]
        public virtual string tel { get; set; }
    }
}
