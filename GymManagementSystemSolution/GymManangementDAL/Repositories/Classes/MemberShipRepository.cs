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
    public class MemberShipRepository : IMemberShipRepository
    {
        private readonly GymDbContext _dbContext;

        public MemberShipRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(MemberShip memberShip)
        {
            _dbContext.MembersShips.Add(memberShip);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var memberShip = _dbContext.MembersShips.Find(id);
            if (memberShip is null) return 0;
            _dbContext.MembersShips.Remove(memberShip);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<MemberShip> GetAll() => _dbContext.MembersShips.ToList();
        public MemberShip? GetById(int id) => _dbContext.MembersShips.Find(id);


        public int Update(MemberShip memberShip)
        {
            _dbContext.MembersShips.Update(memberShip);
            return _dbContext.SaveChanges();
        }
    }
}
