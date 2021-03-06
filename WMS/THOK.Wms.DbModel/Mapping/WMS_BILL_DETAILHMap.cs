﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;

namespace THOK.Wms.DbModel.Mapping
{
    public class WMS_BILL_DETAILHMap : EntityTypeConfiguration<WMS_BILL_DETAILH>
    {
        public WMS_BILL_DETAILHMap()
        {
            // Primary Key
            this.HasKey(t => new { t.BILL_NO, t.ITEM_NO });

            // Properties
            this.Property(t => t.BILL_NO)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ITEM_NO)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PRODUCT_CODE)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.IS_MIX)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.FPRODUCT_CODE)
                .HasMaxLength(30);
            this.Property(t => t.CELL_CODE)
                .HasMaxLength(8);
            this.Property(t => t.INITIAL_BARCODE)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("WMS_BILL_DETAILH", "HNXC");
            this.Property(t => t.BILL_NO).HasColumnName("BILL_NO");
            this.Property(t => t.ITEM_NO).HasColumnName("ITEM_NO");
            this.Property(t => t.PRODUCT_CODE).HasColumnName("PRODUCT_CODE");
            this.Property(t => t.WEIGHT).HasColumnName("WEIGHT");
            this.Property(t => t.REAL_WEIGHT).HasColumnName("REAL_WEIGHT");
            this.Property(t => t.PACKAGE_COUNT).HasColumnName("PACKAGE_COUNT");
            this.Property(t => t.NC_COUNT).HasColumnName("NC_COUNT");
            this.Property(t => t.IS_MIX).HasColumnName("IS_MIX");
            this.Property(t => t.FPRODUCT_CODE).HasColumnName("FPRODUCT_CODE");
            this.Property(t => t.FORDER).HasColumnName("FORDER");
            this.Property(t => t.CELL_CODE).HasColumnName("CELL_CODE");
            this.Property(t => t.INITIAL_BARCODE).HasColumnName("INITIAL_BARCODE");
        }
    }
}
