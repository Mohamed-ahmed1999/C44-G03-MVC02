using GymManangementDAL.Data.Contexts;
using GymManangementDAL.Entities;
using GymManangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManangementDAL.Repositories.Classes
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly GymDbContext _dbContext;

        public HealthRecordRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(HealthRecord healthRecord)
        {
            _dbContext.HealthRecords.Add(healthRecord);
            return _dbContext.SaveChanges();
        }

        public int Delete(HealthRecord record)
        {
            _dbContext.HealthRecords.Remove(record);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<HealthRecord> GetAll() => _dbContext.HealthRecords.ToList();

        public HealthRecord? GetById(int id) => _dbContext.HealthRecords.Find(id);
        public int Update(HealthRecord HealthRecord)
        {
            _dbContext.HealthRecords.Update(HealthRecord);
            return _dbContext.SaveChanges();
        }
    }
}
