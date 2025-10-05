using GymManangementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManangementDAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        IEnumerable<Plan> GetAll();
        Plan? GetById(int id);
        int Add(Plan plan);
        int Update(Plan plan);
        int Delete(Plan plan);
    }
}
