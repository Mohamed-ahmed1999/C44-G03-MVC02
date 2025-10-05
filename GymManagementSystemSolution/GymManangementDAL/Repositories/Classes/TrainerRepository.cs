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
    public class TrainerRepository : ITrainerRepository
    {
        private readonly GymDbContext _DbContext;

        public TrainerRepository(GymDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public int Add(Trainer trainer)
        {
            _DbContext.Trainers.Add(trainer);
            return _DbContext.SaveChanges();
        }

        public int Delete(Trainer trainer)
        {
            _DbContext.Trainers.Remove(trainer);
            return _DbContext.SaveChanges();
        }

        public IEnumerable<Trainer> GetAll() =>  _DbContext.Trainers.ToList();

        public Trainer? GetById(int Id) => _DbContext.Trainers.Find(Id);

        public int Update(Trainer trainer)
        {
            _DbContext.Trainers.Update(trainer);
            return _DbContext.SaveChanges();
        }
    }
}
