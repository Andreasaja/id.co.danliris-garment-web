﻿using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingIns.Configs
{
    public class GarmentSewingInConfig : IEntityTypeConfiguration<GarmentSewingInReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSewingInReadModel> builder)
        {
            builder.ToTable("GarmentSewingIns");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.SewingInNo).HasMaxLength(25);
            builder.Property(a => a.UnitFromCode).HasMaxLength(25);
            builder.Property(a => a.UnitFromName).HasMaxLength(100);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}