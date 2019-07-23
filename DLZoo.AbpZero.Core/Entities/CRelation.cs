using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTempProject.Entities
<<<<<<< HEAD
{
=======
{ 
>>>>>>> b27545f59da83ec2825dd5331bc5a9bd30fb3214
    [Table("relation")]
   public class CRelation:Entity<int>
    {
        public virtual int? customer_id { get; set; }
        public virtual int? site_id { get; set; }
    }
}
