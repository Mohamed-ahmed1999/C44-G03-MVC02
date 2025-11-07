using GymManagementBLL.ViewModels.MemberShipViewModels;
using GymManangementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interface
{
    public interface IMemberShipService
    {
        IEnumerable<MemberShipViewModel> GetAllActiveMemberShips();
        bool CreateMemberShip(int memberId , int planId);
        bool DeleteMemberShip(int memberId, int planId);
    }
}
