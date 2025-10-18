using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManangementDAL.Entities;
using GymManangementDAL.Repositories.Classes;
using GymManangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool CreateTrainer(CreateTrainerViewModel createdTrainer)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<Trainer>();

                if (IsEmailExists(createdTrainer.Email) || IsPhoneExists(createdTrainer.Phone)) return false;
                var Trainer = new Trainer()
                {
                    Name = createdTrainer.Name,
                    Email = createdTrainer.Email,
                    Phone = createdTrainer.Phone,
                    DateOfBirth = createdTrainer.DateOfBirth,
                    Specialties = createdTrainer.Specialties,
                    Gender = createdTrainer.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createdTrainer.BuildingNumber,
                        City = createdTrainer.City,
                        Street = createdTrainer.Street,
                    }
                };


                Repo.Add(Trainer);

                return _unitOfWork.SaveChanges() > 0;


            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainers is null || !Trainers.Any()) return [];

            return Trainers.Select(X => new TrainerViewModel
            {
                Id = X.Id,
                Name = X.Name,
                Email = X.Email,
                Phone = X.Phone,
                Specialties = X.Specialties.ToString()
            });
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (Trainer is null) return null;


            return new TrainerViewModel
            {
                Email = Trainer.Email,
                Name = Trainer.Name,
                Phone = Trainer.Phone,
                Specialties = Trainer.Specialties.ToString()
            };
        }
        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer == null) return null;

            return new TrainerToUpdateViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Street = trainer.Address.Street,
                BuildingNumber = trainer.Address.BuildingNumber,
                City = trainer.Address.City,
                Specialties = trainer.Specialties
            };
        }
        public bool RemoveTrainer(int trainerId)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToRemove = Repo.GetById(trainerId);
            if (TrainerToRemove is null || HasActiveSessions(trainerId)) return false;
            Repo.Delete(TrainerToRemove);
            return _unitOfWork.SaveChanges() > 0;
        }

        public bool UpdateTrainerDetails(TrainerToUpdateViewModel updatedTrainer, int trainerId)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainer = repo.GetById(trainerId);

            if (trainer == null || IsEmailExists(updatedTrainer.Email) || IsPhoneExists(updatedTrainer.Phone))
                return false;

            trainer.Email = updatedTrainer.Email;
            trainer.Phone = updatedTrainer.Phone;
            trainer.Address.BuildingNumber = updatedTrainer.BuildingNumber;
            trainer.Address.Street = updatedTrainer.Street;
            trainer.Address.City = updatedTrainer.City;
            trainer.Specialties = updatedTrainer.Specialties;
            trainer.UpdatedAt = DateTime.Now;

            repo.Update(trainer);
            return _unitOfWork.SaveChanges() > 0;
        }


        #region Helper Methods

        private bool IsEmailExists(string email)
        {
            var existing = _unitOfWork.GetRepository<Trainer>().GetAll(
                m => m.Email == email).Any();
            return existing;
        }

        private bool IsPhoneExists(string phone)
        {
            var existing = _unitOfWork.GetRepository<Trainer>().GetAll(
                m => m.Phone == phone).Any();
            return existing;
        }

        private bool HasActiveSessions(int Id)
        {
            var activeSessions = _unitOfWork.GetRepository<Session>().GetAll(
               s => s.TrainerId == Id && s.StartDate > DateTime.Now).Any();
            return activeSessions;
        }
        #endregion
    }
}
