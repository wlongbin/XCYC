using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

namespace THOK.Wms.DbModel.Mapping
{
    public class WMS_REPLACE_DETAILMap : EntityTypeConfiguration<WMS_REPLACE_DETAIL>
    {
        public WMS_REPLACE_DETAILMap()
        {
            // Primary Key
            this.HasKey(t => new { t.BILL_NO, t.PRODUCT_BARCODE });

            // Properties
            this.Property(t => t.BILL_NO)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.PRODUCT_CODE)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PRODUCT_BARCODE)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("WMS_REPLACE_DETAIL", "HNXC");
            this.Property(t => t.BILL_NO).HasColumnName("BILL_NO");
            this.Property(t => t.PRODUCT_CODE).HasColumnName("PRODUCT_CODE");
            this.Property(t => t.PRODUCT_BARCODE).HasColumnName("PRODUCT_BARCODE");
            this.Property(t => t.WEIGHT).HasColumnName("WEIGHT");
            this.Property(t => t.REAL_WEIGHT).HasColumnName("REAL_WEIGHT");
        }
    }
}
