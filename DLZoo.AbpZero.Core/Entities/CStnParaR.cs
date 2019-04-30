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
    [Table("stn_para_r")]
    public class CStnParaR : Entity<int>
    {
       // [paraid]       VARCHAR(50) NOT NULL,
        [Required]
        [MaxLength(50)]
        public virtual string paraid { get; set; }
        // [stid]         INT NULL
        public virtual int? stid { get; set; }

        // [paraTypeCode] VARCHAR(4)  NULL,
        [MaxLength(4)]
        public virtual string paraTypeCode { get; set; }

        // [attrCode1]    VARCHAR(10) NULL,
        [MaxLength(10)]
        public virtual string attrCode1 { get; set; }

        // [attrCode2]    VARCHAR(10) NULL,
        [MaxLength(10)]
        public virtual string attrCode2 { get; set; }

        // [attrName2]    VARCHAR(10) NULL,
        [MaxLength(10)]
        public virtual string attrName2 { get; set; }

        // [position]     INT NULL,
        public virtual int? position { get; set; }
        // [stcd]         VARCHAR(20) NULL,
        [MaxLength(20)]
        public virtual string stcd { get; set; }

        // [rtucode]      VARCHAR(32) NULL,
        [MaxLength(32)]
        public virtual string rtucode { get; set; }

    }
}
