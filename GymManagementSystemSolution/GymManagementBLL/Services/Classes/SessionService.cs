using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymManangementDAL.Entities;
using GymManangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel CreateSession)
        {
            try
            {

                if (!IsTrainerExists(CreateSession.TrainerId)) return false;
                if (!IsCategoryExists(CreateSession.CategoryId)) return false;
                if (!IsDateTimeValid(CreateSession.StartDate, CreateSession.EndDate)) return false;

                if (CreateSession.Capacity < 0 || CreateSession.Capacity > 25) return false;

                var SessionEntity = _mapper.Map<Session>(CreateSession);
                _unitOfWork.GetRepository<Session>().Add(SessionEntity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create Session Failed : {ex}");
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (!Sessions.Any()) return [];

            var MappedSessions = _mapper.Map<IEnumerable<Session> , IEnumerable<SessionViewModel>>(Sessions);
            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            }
            return MappedSessions;

        }

        public SessionViewModel? GetSessionById(int sessionid)
        {
            var Session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionid);
            if (Session is null) return null;
           
            var MappedSession = _mapper.Map<Session, SessionViewModel>(Session);
            MappedSession.AvailableSlots = MappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(MappedSession.Id);
            return MappedSession;


        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var Session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (IsSessionAvaiableForUpdated(Session!)) return null;

            return _mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(UpdateSessionViewModel updateSession, int sessionId)
        {
            try
            {
                var repo = _unitOfWork.GetRepository<Session>();
                var Session = repo.GetById(sessionId);

                if (!IsSessionAvaiableForUpdated(Session!)) return false;
                if (!IsTrainerExists(updateSession.TrainerId)) return false;
                if (!IsDateTimeValid(updateSession.StartDate, updateSession.EndDate)) return false;

                _mapper.Map(updateSession, Session);
                repo.Update(Session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Session Failed : {ex}");
                return false;
            }
        }

        public bool RemoveSession(int sessionId)
        {
            try
            {
                var repo = _unitOfWork.GetRepository<Session>();
                var session = repo.GetById(sessionId);
                if (!IsSessionAvaiableForRemove(session!)) return false;

                repo.Delete(session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Remove Session Failed : {ex}");
                return false;
            }
        }

        public IEnumerable<TrainerSelectViewModel> GetTrainerForDropDown()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers); 
        }

        public IEnumerable<CategorySelectViewModel> GetCategoryForDropDown()
        {
            var categories = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }

        #region Helper Method


        private bool IsSessionAvaiableForRemove(Session session)
        {
          return session.EndDate < DateTime.Now
                &&
                _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) == 0;
        }

        private bool IsSessionAvaiableForUpdated(Session session)
        {
         
            return session.EndDate > DateTime.Now &&
                _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) == 0;
        }


        private bool IsTrainerExists(int trainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(trainerId) is not null;
        }

        private bool IsCategoryExists(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }

        private bool IsDateTimeValid(DateTime startDate, DateTime endDate)
        {
            return endDate > startDate && DateTime.Now < startDate;
        }

        #endregion

    }
}
