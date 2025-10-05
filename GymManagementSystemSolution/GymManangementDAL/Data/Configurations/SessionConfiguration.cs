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
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("SessionCapacityCheck", "Capacity Between 1 and 29");
                Tb.HasCheckConstraint("SessionEndDateCheck", "EndDate > StartDate");
            });

            builder.HasOne(x => x.SessionCategory)
                .WithMany(x => x.Sessions)
                .HasForeignKey(x => x.CategoryId);

            builder.HasOne(x => x.SessionTrainer)
                .WithMany(x => x.TrainerSession)
                .HasForeignKey(x => x.TrainerId);
        }
    }
}
