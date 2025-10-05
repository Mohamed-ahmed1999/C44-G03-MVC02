using GymManangementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManangementDAL.Data.Configurations
{
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(X => X.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(X => X.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            builder.Property(X => X.Phone)
               .HasColumnType("varchar")
               .HasMaxLength(11);

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserValidEmailCheck", "Email Like '_%@_%._%' ");
                Tb.HasCheckConstraint("GymUserValidPhoneCheck", "Phone Like '01%' and Phone Not Like '%[^0-9]%' ");
            });

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x=> x.Phone).IsUnique();

            builder.OwnsOne(x => x.Address, AddressBuilder =>
            {
                AddressBuilder.Property(X=>X.Street)
                .HasColumnName("Street")
                .HasColumnType("varchar")
                .HasMaxLength (30);

                AddressBuilder.Property(X=>X.City)
                .HasColumnName("City")
                .HasColumnType("varchar")
                .HasMaxLength(30);

                AddressBuilder.Property(x => x.BuildingNumber)
                .HasColumnName("BuildingNumber");
            });
        }
    }
}
