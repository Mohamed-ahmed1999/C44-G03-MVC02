using GymManangementDAL.Data.Contexts;
using GymManangementDAL.Entities;
using GymManangementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManangementDAL.Repositories.Classes
{
    public class MemberSessionRepository : IMemberSessionRepository
    {
        private readonly GymDbContext _DbContext;

        public MemberSessionRepository(GymDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public int Add(MemberSession session)
        {
            _DbContext.MembersSessions.Add(session);
            return _DbContext.SaveChanges();
        }

        public int Delete(MemberSession session)
        {
            _DbContext.MembersSessions.Remove(session);
            return _DbContext.SaveChanges();
        }

        public IEnumerable<MemberSession> GetAll()
        => _DbContext.MembersSessions.ToList();

        public MemberSession? GetById(int id)
            => _DbContext.MembersSessions.Find(id);

        public int Update(MemberSession session)
        {
            _DbContext.MembersSessions.Update(session);
            return _DbContext.SaveChanges();
        }
    }
}
