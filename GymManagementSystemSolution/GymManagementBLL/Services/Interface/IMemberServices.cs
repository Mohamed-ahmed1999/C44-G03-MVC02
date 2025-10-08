using GymManagementBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interface
{
    internal interface IMemberServices
    {
        IEnumerable<MemberViewModel> GetAllMembers();

        bool CreateMember(CreateMemberViewModel createdMember);

        MemberViewModel? GetMember(int MemberId);

        HealthRecordViewModel? GetMemberHealthRecordDetials(int MemberId);

        MemberToUpdateViewModel? GetMemberToUpdate(int MemberId);

        bool UpdateMemberDetails(int Id , MemberToUpdateViewModel UpdatedMember);

        bool RemoveMember(int MemberId);
    }
}
