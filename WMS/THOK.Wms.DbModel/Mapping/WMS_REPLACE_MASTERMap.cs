using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

namespace THOK.Wms.DbModel.Mapping
{
    public class WMS_REPLACE_MASTERMap : EntityTypeConfiguration<WMS_REPLACE_MASTER>
    {
        public WMS_REPLACE_MASTERMap()
        {
            // Primary Key
            this.HasKey(t => t.BILL_NO);

            // Properties
            this.Property(t => t.BILL_NO)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.OPERATER)
                .HasMaxLength(10);

            this.Property(t => t.REASON)
                .HasMaxLength(500);
            this.Property(t => t.STATE)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.REMARK)
                .HasMaxLength(500);
            this.Property(t => t.CHECKER)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("WMS_REPLACE_MASTER", "HNXC");
            this.Property(t => t.BILL_NO).HasColumnName("BILL_NO");
            this.Property(t => t.BILL_DATE).HasColumnName("BILL_DATE");
            this.Property(t => t.OPERATER).HasColumnName("OPERATER");
            this.Property(t => t.OPERATE_DATE).HasColumnName("OPERATE_DATE");
            this.Property(t => t.REASON).HasColumnName("REASON");
            this.Property(t => t.STATE).HasColumnName("STATE");
            this.Property(t => t.REMARK).HasColumnName("REMARK");
            this.Property(t => t.CHECKER).HasColumnName("CHECKER");
            this.Property(t => t.CHECK_DATE).HasColumnName("CHECK_DATE");
            this.Property(t => t.TRANSER).HasColumnName("TRANSER");
            this.Property(t => t.TRANS_DATE).HasColumnName("TRANS_DATE");
        }
    }
}
