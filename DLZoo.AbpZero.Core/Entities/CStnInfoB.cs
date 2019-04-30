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
    [Table("stn_info_b")]
    public class CStnInfoB : Entity<int>
    {
        [Required]
        [Column("stid")]
        public override int Id { get; set; }
        //     [areaCode]    VARCHAR(20)  NOT NULL,
        [MaxLength(20)]
        public virtual string areaCode { get; set; }
        //[areaName]    VARCHAR(50)  NULL,
        [MaxLength(50)]
        public virtual string areaName { get; set; }
        // [areaDesc]    VARCHAR(100) NULL,
        [MaxLength(100)]
        public virtual string areaDesc { get; set; }
        // [lgtd]        FLOAT(53)    NULL,
        public virtual double? lgtd { get; set; }
        // [lttd]        FLOAT(53)    NULL,
        public virtual double? lttd { get; set; }
        // [addvcd]      VARCHAR(6)   NULL,
        [MaxLength(6)]
        public virtual string addvcd { get; set; }
        // [iconurl]     VARCHAR(20)  NULL,
        [MaxLength(20)]
        public virtual string iconurl { get; set; }
        // [projectCode] VARCHAR(32)  NULL,
        [MaxLength(32)]
        public virtual string projectCode { get; set; }
        // [showLevel]    INT NULL,
        public virtual int? showLevel { get; set; }
        // [stlc]        VARCHAR(100) NULL,
        [MaxLength(100)]
        public virtual string stlc { get; set; }
        // [posCode]     VARCHAR(20)  NULL,
        [MaxLength(20)]
        public virtual string posCode { get; set; }
        // [posName]     VARCHAR(50)  NULL,
        [MaxLength(50)]
        public virtual string posName { get; set; }
        // [stType]      VARCHAR(32)  NULL,
        [MaxLength(32)]
        public virtual string stType { get; set; }
        // [photo]       VARCHAR(50)  NULL,
        [MaxLength(50)]
        public virtual string photo { get; set; }
        // [parentid]    INT NULL,
        public virtual int? parentid { get; set; }
        // [temporary]   INT NULL,
        public virtual int? temporary { get; set; }
        // [artificial]  INT NULL,
        public virtual int? artificial { get; set; }
        // [contacter]   VARCHAR(50)  NULL,
        [MaxLength(50)]
        public virtual string contacter { get; set; }
        // [telephone]   VARCHAR(15)  NULL,
        [MaxLength(15)]
        public virtual string telephone { get; set; }
        // [installdate]    DATETIME NULL,
        public virtual DateTime installdate { get; set; }
        // [holeDepth]   FLOAT(53)    NULL,
        public virtual double? holeDepth { get; set; }
    }
}
