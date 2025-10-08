using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManangementDAL.Entities;
using GymManangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    internal class MemberService : IMemberServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateMember(CreateMemberViewModel createdMember)
        {
            try
            {

                if (IsEmailExists(createdMember.Email) || IsPhoneExists(createdMember.Phone)) return false;

                var memebr = new Member()
                {
                    Name = createdMember.Name,
                    Email = createdMember.Email,
                    Phone = createdMember.Phone,
                    Gender = createdMember.Gender,
                    DateOfBirth = createdMember.DateOfBirth,
                    Address = new Address()
                    {
                        BuildingNumber = createdMember.BuildingNumber,
                        City = createdMember.City,
                        Street = createdMember.Street,
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createdMember.HealthRecordViewModel.Height,
                        Weight = createdMember.HealthRecordViewModel.Weight,
                        BloodType = createdMember.HealthRecordViewModel.BloodType,
                        Note = createdMember.HealthRecordViewModel.Note
                    }


                };

                 _unitOfWork.GetRepository<Member>().Add(memebr);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
       
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = _unitOfWork.GetRepository<Member>().GetAll();
            if (Members is null || !Members.Any()) return [];

                var MemberViewModels = Members.Select(x => new MemberViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Phone = x.Phone,
                    Photo = x.Photo,
                    Gender = x.Gender.ToString()
                });
            return MemberViewModels;
        }

        public MemberViewModel? GetMember(int MemberId)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;

            var ViewModel = new MemberViewModel()
            {
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Gender = Member.Gender.ToString(),
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
                Adress = $"{Member.Address.BuildingNumber} - {Member.Address.Street} - {Member.Address.City}",
                Photo = Member.Photo
            };

            var ActivememberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(x => x.MemberId == MemberId && x.Status == "Active").FirstOrDefault();
            if (ActivememberShip is not null)
            {
                ViewModel.MemberShipStartDate = ActivememberShip.CreatedAt.ToShortDateString();
                ViewModel.MemberShipEndDate = ActivememberShip.EndDate.ToShortDateString();

                var Plan = _unitOfWork.GetRepository<Plan>().GetById(ActivememberShip.PlanId);
                ViewModel.PlanName = Plan?.Name;

            }

            return ViewModel;

        }

        public HealthRecordViewModel? GetMemberHealthRecordDetials(int MemberId)
        {
            var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecord is null) return null;

            return new HealthRecordViewModel()
            {
                BloodType = MemberHealthRecord.BloodType,
                Height = MemberHealthRecord.Height,
                Note = MemberHealthRecord.Note,
                Weight = MemberHealthRecord.Weight
            };
            
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;
            return new MemberToUpdateViewModel()
            {
                Email = Member.Email,
                Name = Member.Name,
                Phone = Member.Phone,
                Photo = Member.Photo,
                BuildingNumber = Member.Address.BuildingNumber,
                City = Member.Address.City,
                Street = Member.Address.Street,
            };

        }

        public bool RemoveMember(int MemberId)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();

            var Member = MemberRepo.GetById(MemberId);
            if (Member is null) return false;
            var HasActiveMemberSessions = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(x => x.MemberId == MemberId && x.Session.StartDate > DateTime.Now).Any();

            if (HasActiveMemberSessions) return false;


            var MembershipRepo = _unitOfWork.GetRepository<MemberShip>();
            var Memberships = MembershipRepo.GetAll(x => x.MemberId == MemberId);

            try 
            {
                if (Memberships.Any())
                {
                    foreach (var membership in Memberships)
                    {
                        MembershipRepo.Delete(membership);
                    }
                }
                MemberRepo.Delete(Member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch 
            {
                return false;
            }
        }

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel UpdatedMember)
        {
            try 
            {
                if (IsEmailExists(UpdatedMember.Email) || IsPhoneExists(UpdatedMember.Phone)) return false;

                var repo = _unitOfWork.GetRepository<Member>();

                var Member = repo.GetById(Id);
                if (Member is null) return false;
                Member.Email = UpdatedMember.Email;
                Member.Phone = UpdatedMember.Phone;
                Member.Address.BuildingNumber = UpdatedMember.BuildingNumber;
                Member.Address.City = UpdatedMember.City;
                Member.Address.Street = UpdatedMember.Street;
                Member.UpdatedAt = DateTime.Now;
                repo.Update(Member) ;

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
            return _unitOfWork.GetRepository<Member>().GetAll(x => x.Email == email).Any();
            
        }

        private bool IsPhoneExists(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(x => x.Phone == phone).Any();

        }
        #endregion
    }
}
