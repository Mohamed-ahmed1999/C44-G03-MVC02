using GymManangementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManangementDAL.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        //GetAll
        IEnumerable<Member> GetAll();

        //Get by id
        Member? GetById(int Id);

        //Add
        int Add(Member member);

        //update
        int update(Member member);

        //delete
        int Delete(int Id);

    }
}
