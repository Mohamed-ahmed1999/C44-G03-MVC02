using GymManagementBLL.ViewModels.MemberSessionViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interface
{
    public interface IMemberSessionService
    {
        IEnumerable<MemberSessionViewModel> GetMembersForUpcomingSession(int sessionId);
        IEnumerable<MemberSessionViewModel> GetMembersForOngoingSession(int sessionId);
        bool CreateMemberSession(CreateMemberSessionViewModel createBooking);
        bool CancelMemberSession(int memberId, int sessionId);
        bool MarkAttendance(int memberId, int sessionId);

    }
}
