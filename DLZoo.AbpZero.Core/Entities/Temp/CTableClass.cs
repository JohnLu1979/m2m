using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace MyTempProject.Entities.Temp
{
    [Table("Table")]
    public class CTableClass : Entity<long>
    {
        public const int MaxNameStringLength = 100;
        public const int MaxRemarkStringLength = 300;

        [Required]
        public virtual int Id
        {
            get; set;
        }
        [MaxLength(MaxNameStringLength)]
        public virtual string Column_A
        {
            get; set;
        }
        public virtual decimal Column_B
        {
            get; set;
        }
    }
}

