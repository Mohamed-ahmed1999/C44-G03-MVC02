using GymManangementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManangementDAL.Repositories.Interfaces
{
    public interface IMemberSessionRepository
    {
        IEnumerable<MemberSession> GetAll();
        MemberSession? GetById(int id);
        int Add(MemberSession session);
        int Update(MemberSession session);
        int Delete(MemberSession session);
    }
}
