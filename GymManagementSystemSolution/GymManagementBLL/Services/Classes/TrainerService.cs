using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManangementDAL.Entities;
using GymManangementDAL.Repositories.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerServices
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(UnitOfWork unitOfWork , IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any()) return [];

            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }

        public TrainerViewModel? GetTrainerDetails(int TrainerId)
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainers is null) return null;
            return _mapper.Map<TrainerViewModel>(trainers);
        }

        public bool UpdateTrainerDetails(int Id, TrainerToUpdateViewModel UpdatedTrainer)
        {
            try
            {
                if (IsEmailExists(UpdatedTrainer.Email) || IsPhoneExists(UpdatedTrainer.Phone)) return false;

                var repo = _unitOfWork.GetRepository<Trainer>();
                var trainer = repo.GetById(Id);
                if (trainer is null) return false;

                _mapper.Map(UpdatedTrainer, trainer);

                repo.Update(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
      

        public bool CreateTrainer(CreateTrainerViewModel createdTrainer)
        {
            try 
            {
                if (IsEmailExists(createdTrainer.Email) || IsPhoneExists(createdTrainer.Phone)) return false;
                
                if (createdTrainer.Specialization == 0)
                    return false;
                var trainer = _mapper.Map<Trainer>(createdTrainer);
                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch 
            {
                return false;
            }
        }

        public bool RemoveTrainer(int TrainerId)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainer = repo.GetById(TrainerId);
            if (trainer is null) return false;

            var HasActiveTrainerSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(x => x.TrainerId == TrainerId && x.StartDate > DateTime.Now).Any();

            if (HasActiveTrainerSessions) return false;

            try
            {
                repo.Delete(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        #region Helper Methods

        private bool IsEmailExists(string email)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Email == email).Any();

        }

        private bool IsPhoneExists(string phone)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Phone == phone).Any();

        }
        #endregion 
    }
}
