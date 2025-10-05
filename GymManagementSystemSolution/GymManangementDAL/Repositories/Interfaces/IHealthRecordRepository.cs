using GymManangementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManangementDAL.Repositories.Interfaces
{
    public interface IHealthRecordRepository
    {
        IEnumerable<HealthRecord> GetAll();

        HealthRecord? GetById(int id);

        int Add(HealthRecord healthRecord);

        int Update(HealthRecord HealthRecord);

        int Delete(HealthRecord HealthRecord);
    }
}
