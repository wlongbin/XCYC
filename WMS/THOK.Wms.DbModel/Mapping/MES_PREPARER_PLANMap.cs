using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

namespace THOK.Wms.DbModel.Mapping
{
    public  class MES_PREPARER_PLANMap : EntityTypeConfiguration<MES_PREPARER_PLAN>
    {
        public MES_PREPARER_PLANMap()
        {
            // Primary Key
            this.HasKey(t => new { t.PLAN_DATE,t.MAT_CD });
            // Table & Column Mappings
            this.ToTable("MES_PREPARER_PLAN", "HNXC");
            this.Property(t => t.PLAN_DATE).HasColumnName("PLAN_DATE");
            this.Property(t => t.REC_DT).HasColumnName("REC_DT");
            this.Property(t => t.RECEIVER).HasColumnName("RECEIVER");
            this.Property(t => t.SEND_DATETIME).HasColumnName("SEND_DATETIME");
            this.Property(t => t.SENDER).HasColumnName("SENDER");
            this.Property(t => t.MAT_CD).HasColumnName("MAT_CD");
            this.Property(t => t.LATE_PREPARE_DATETIME).HasColumnName("LATE_PREPARE_DATETIME");
            this.Property(t => t.BATCHES).HasColumnName("BATCHES");
        }
    }
}
