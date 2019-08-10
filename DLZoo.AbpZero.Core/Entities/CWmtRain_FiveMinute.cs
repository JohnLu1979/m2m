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
    [Table("wmt_rain_5min")]
    public class CWmtRain_FiveMinute : Entity<int>
    {
        //    [stcd]        VARCHAR(10) NULL,
        [MaxLength(10)]
        public virtual string stcd { get; set; }
        //[drp]   FLOAT(53)   NULL,
        public virtual double? drp { get; set; }
        //[tm]    DATETIME NULL,
        public virtual DateTime tm { get; set; }
        //[wth] [varchar] (1) NULL,
        public virtual string wth { get; set; }
        //[uniquemark]  VARCHAR(32) NULL,
        [MaxLength(32)]
        public virtual string uniquemark { get; set; }
        //[gentm]    DATETIME DEFAULT(getdate()) NULL,
        public virtual DateTime gentm { get; set; }
        //[dataflag] [varchar] (1) NULL,
        public virtual string dataflag { get; set; }
        //[againflag] [varchar] (1) NULL,
        public virtual string againflag { get; set; }
    }
}
