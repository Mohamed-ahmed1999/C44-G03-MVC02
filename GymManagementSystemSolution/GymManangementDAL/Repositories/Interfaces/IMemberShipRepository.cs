using GymManangementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManangementDAL.Repositories.Interfaces
{
    public interface IMemberShipRepository
    {
        IEnumerable<MemberShip> GetAll();
        MemberShip? GetById(int id);
        int Add(MemberShip memberShip);
        int Update(MemberShip memberShip);
        int Delete(int id);
    }
}
