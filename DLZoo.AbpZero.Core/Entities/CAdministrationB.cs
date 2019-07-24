using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.Entities
{
    [Table("adm_administration_b")]
    public class CAdministrationB
    {
        [Required]
        [MaxLength(50)]
        public virtual string addvcd { get; set; }
        
        public virtual string addvname { get; set; }

        public virtual string parentcd { get; set; }

        public virtual string leafflag { get; set; }

        public virtual string levels { get; set; }

        public virtual string areaid { get; set; }
    }
}
