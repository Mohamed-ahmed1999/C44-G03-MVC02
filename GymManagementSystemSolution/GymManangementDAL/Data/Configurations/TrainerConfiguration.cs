﻿using GymManangementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManangementDAL.Data.Configurations
{
    internal class TrainerConfiguration : GymUserConfiguration<Trainer> , IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {

            builder.Property(x => x.CreatedAt)
                .HasColumnName("HireDate")
                .HasDefaultValueSql("GETDATE()");


            base.Configure(builder);
        }

    }
}
