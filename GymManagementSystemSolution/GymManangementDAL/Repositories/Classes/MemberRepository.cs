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
    public class MemberRepository : IMemberRepository
    {
        private readonly GymDbContext _dbContext;

        public MemberRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }   
        public int Add(Member member)
        {
            _dbContext.Members.Add(member);
            return _dbContext.SaveChanges();
        }

        public int Delete(int Id)
        {
            var Member = _dbContext.Members.Find(Id);
            if (Member is null) return 0;
            _dbContext.Members.Remove(Member);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Member> GetAll() => _dbContext.Members.ToList();

        public Member? GetById(int Id) => _dbContext.Members.Find(Id);


        public int update(Member member)
        {
            _dbContext.Members.Update(member);
            return _dbContext.SaveChanges();
        }
    }
}
