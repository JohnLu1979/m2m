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
    [Table("user")]
    public class CUser : Entity<int>
    {
        [MaxLength(50)] 
        public virtual string username { get; set; }
        [MaxLength(50)]
        public virtual string password { get; set; }
        [MaxLength(50)]
        public virtual string address { get; set; }
        [MaxLength(50)]
        public virtual string tel { get; set; }
    }
}
