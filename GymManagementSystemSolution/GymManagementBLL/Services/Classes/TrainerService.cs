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
    internal class TrainerService : ITrainerServices
    {
        private readonly UnitOfWork _unitOfWork;

        public TrainerService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any()) return [];

            return trainers.Select(t => new TrainerViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialization = t.Specialties
            });
        }

        public TrainerViewModel? GetTrainerDetails(int TrainerId)
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainers is null) return null;
            return new TrainerViewModel()
            {
                Id = trainers.Id,
                Name = trainers.Name,
                Email = trainers.Email,
                Phone = trainers.Phone,
                Specialization = trainers.Specialties
            };
        }

        public bool UpdateTrainerDetails(int Id, TrainerToUpdateViewModel UpdatedTrainer)
        {
            try
            {
                if (IsEmailExists(UpdatedTrainer.Email) || IsPhoneExists(UpdatedTrainer.Phone)) return false;

                var repo = _unitOfWork.GetRepository<Trainer>();
                var trainer = repo.GetById(Id);
                if (trainer is null) return false;

                trainer.Email = UpdatedTrainer.Email;
                trainer.Phone = UpdatedTrainer.Phone;
                trainer.Specialties = UpdatedTrainer.Specialization;
                trainer.Address.BuildingNumber = UpdatedTrainer.BuildingNumber;
                trainer.Address.Street = UpdatedTrainer.Street;
                trainer.Address.City = UpdatedTrainer.City;
                trainer.UpdatedAt = DateTime.Now;

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
                var trainer = new Trainer()
                {
                    Name = createdTrainer.Name,
                    Email = createdTrainer.Email,   
                    Phone = createdTrainer.Phone,
                    Gender = createdTrainer.Gender,
                    DateOfBirth = createdTrainer.DateOfBirth,
                    Specialties = createdTrainer.Specialization,
                    Address = new Address()
                    {
                        BuildingNumber = createdTrainer.BuildingNumber,
                        City = createdTrainer.City,
                        Street = createdTrainer.Street,
                    }
                };
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
