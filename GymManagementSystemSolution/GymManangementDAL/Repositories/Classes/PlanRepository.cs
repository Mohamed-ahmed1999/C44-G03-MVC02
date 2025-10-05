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
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext _DbContext;

        public PlanRepository(GymDbContext dbContext)
        {
            _DbContext = dbContext;
        }
        public int Add(Plan plan)
        {
            _DbContext.Plans.Add(plan);
            return _DbContext.SaveChanges();
        }

        public int Delete(Plan plan)
        {
           
            _DbContext.Plans.Remove(plan);
            return _DbContext.SaveChanges();
        }

        public IEnumerable<Plan> GetAll() => _DbContext.Plans.ToList();
        public Plan? GetById(int id) => _DbContext.Plans.Find(id);

        public int Update(Plan plan)
        {
            _DbContext.Plans.Update(plan);
            return _DbContext.SaveChanges();
        }
    }
}
