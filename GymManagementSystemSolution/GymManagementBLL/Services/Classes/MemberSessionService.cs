using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberSessionViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManangementDAL.Entities;
using GymManangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberSessionService : IMemberSessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberSessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateMemberSession(CreateMemberSessionViewModel model)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(model.MemberId);
            var session = _unitOfWork.GetRepository<Session>().GetById(model.SessionId);

            if (member == null || session == null)
                return false;

            var hasActiveMembership = _unitOfWork.GetRepository<MemberShip>()
                .GetAll(ms => ms.MemberId == model.MemberId && ms.EndDate > DateTime.Now)
                .Any();
            if (!hasActiveMembership)
                return false;

            if (session.StartDate <= DateTime.Now)
                return false;

            var bookedCount = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(ms => ms.SessionId == model.SessionId)
                .Count();

            if (bookedCount >= session.Capacity)
                return false;

            var alreadyBooked = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(ms => ms.MemberId == model.MemberId && ms.SessionId == model.SessionId)
                .Any();
            if (alreadyBooked)
                return false;

            var booking = new MemberSession
            {
                MemberId = model.MemberId,
                SessionId = model.SessionId,
                IsAttended = false,
                CreatedAt = DateTime.Now
            };

            _unitOfWork.GetRepository<MemberSession>().Add(booking);
            return _unitOfWork.SaveChanges() > 0;
        }
        public IEnumerable<MemberSessionViewModel> GetMembersForUpcomingSession(int sessionId)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
            if (session == null || session.StartDate <= DateTime.Now)
                return new List<MemberSessionViewModel>();

            var bookings = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(ms => ms.SessionId == sessionId)
                .ToList();
            foreach (var booking in bookings)
            {
                booking.Member = _unitOfWork.GetRepository<Member>().GetById(booking.MemberId);
                booking.Session = session;
            }

            return _mapper.Map<IEnumerable<MemberSessionViewModel>>(bookings);
        }

        public IEnumerable<MemberSessionViewModel> GetMembersForOngoingSession(int sessionId)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
            if (session == null)
                return new List<MemberSessionViewModel>();

            if (DateTime.Now < session.StartDate || DateTime.Now > session.EndDate)
                return new List<MemberSessionViewModel>();

            var bookings = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(ms => ms.SessionId == sessionId)
                .ToList();

            foreach (var booking in bookings)
            {
                booking.Member = _unitOfWork.GetRepository<Member>().GetById(booking.MemberId)!;
                booking.Session = session;
            }

            return _mapper.Map<IEnumerable<MemberSessionViewModel>>(bookings);
        }
        public bool CancelMemberSession(int memberId, int sessionId)
        {
            var booking = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(ms => ms.MemberId == memberId && ms.SessionId == sessionId).FirstOrDefault();
            if (booking == null)
                return false;

            var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
            if (session == null || session.StartDate <= DateTime.Now)
                return false; 

            _unitOfWork.GetRepository<MemberSession>().Delete(booking);
            return _unitOfWork.SaveChanges() > 0;
        }

        public bool MarkAttendance(int memberId, int sessionId)
        {
            var booking = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(ms => ms.MemberId == memberId && ms.SessionId == sessionId).FirstOrDefault();

            var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
            if (booking == null || session == null)
                return false;

            if (DateTime.Now < session.StartDate || DateTime.Now > session.EndDate)
                return false;

            booking.IsAttended = true;
            _unitOfWork.GetRepository<MemberSession>().Update(booking);
            return _unitOfWork.SaveChanges() > 0;
        }

     
    }
}
