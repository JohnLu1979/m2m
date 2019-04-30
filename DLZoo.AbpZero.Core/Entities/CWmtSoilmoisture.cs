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
    [Table("wmt_soilmoisture")]
    public class CWmtSoilMoisture : Entity<int>
    {
        //    [stcd]        VARCHAR(10) NULL,
        [MaxLength(10)]
        public virtual string stcd { get; set; }
        //[paravalue]   FLOAT(53)   NULL,
        public virtual double? paravalue { get; set; }
        //[collecttime]    DATETIME NULL,
        public virtual DateTime collecttime { get; set; }
        //[systemtime]  DATETIME NULL,
        public virtual DateTime systemtime { get; set; }
        //[uniquemark]  VARCHAR(32) NULL,
        [MaxLength(32)]
        public virtual string uniquemark { get; set; }
        //[gentm]    DATETIME DEFAULT(getdate()) NULL,
        public virtual DateTime gentm { get; set; }
        //[serialnum]   INT          NULL,
        public virtual int? serialnum { get; set; }

    }
}
