using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

namespace THOK.Wms.DbModel.Mapping
{
    public class VIEW_CMD_PRODUCTMap : EntityTypeConfiguration<VIEW_CMD_PRODUCT>
    {
        public VIEW_CMD_PRODUCTMap()
        {
            // Primary Key
            this.HasKey(t => t.PRODUCT_CODE);

            // Properties
            this.Property(t => t.PRODUCT_CODE)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.PRODUCT_NAME)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ORIGINAL_CODE)
                .IsFixedLength()
                .HasMaxLength(12);

            this.Property(t => t.YEARS)
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.GRADE_CODE)
                .IsFixedLength()
                .HasMaxLength(12);

            this.Property(t => t.STYLE_NO)
                .IsFixedLength()
                .HasMaxLength(12);

            this.Property(t => t.MEMO)
                .HasMaxLength(200);

            this.Property(t => t.CATEGORY_CODE)
                .IsRequired()
                .HasMaxLength(12);

            this.Property(t => t.STYLE_NAME)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.CATEGORY_NAME)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.GRADE_NAME)
                .HasMaxLength(20);

            this.Property(t => t.ORIGINAL_NAME)
                .HasMaxLength(20);

            this.Property(t => t.ID)
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.SPECIFICATION)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.LEAF_OTHER_ID)
                .IsFixedLength()
                .HasMaxLength(12);

            this.Property(t => t.STATUS)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("VIEW_CMD_PRODUCT", "HNXC");
            this.Property(t => t.PRODUCT_CODE).HasColumnName("PRODUCT_CODE");
            this.Property(t => t.PRODUCT_NAME).HasColumnName("PRODUCT_NAME");
            this.Property(t => t.ORIGINAL_CODE).HasColumnName("ORIGINAL_CODE");
            this.Property(t => t.YEARS).HasColumnName("YEARS");
            this.Property(t => t.GRADE_CODE).HasColumnName("GRADE_CODE");
            this.Property(t => t.STYLE_NO).HasColumnName("STYLE_NO");
            this.Property(t => t.WEIGHT).HasColumnName("WEIGHT");
            this.Property(t => t.MEMO).HasColumnName("MEMO");
            this.Property(t => t.CATEGORY_CODE).HasColumnName("CATEGORY_CODE");
            this.Property(t => t.STYLE_NAME).HasColumnName("STYLE_NAME");
            this.Property(t => t.CATEGORY_NAME).HasColumnName("CATEGORY_NAME");
            this.Property(t => t.GRADE_NAME).HasColumnName("GRADE_NAME");
            this.Property(t => t.ORIGINAL_NAME).HasColumnName("ORIGINAL_NAME");

            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SPECIFICATION).HasColumnName("SPECIFICATION");
            this.Property(t => t.LEAF_OTHER_ID).HasColumnName("LEAF_OTHER_ID");
            this.Property(t => t.STATUS).HasColumnName("STATUS");

        }
    }
}
