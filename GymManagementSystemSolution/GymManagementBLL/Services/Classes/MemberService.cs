using AutoMapper;
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
    public class MemberService : IMemberServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateMember(CreateMemberViewModel createdMember)
        {
            try
            {

                if (IsEmailExists(createdMember.Email) || IsPhoneExists(createdMember.Phone)) return false;

                var memebr = _mapper.Map<Member>(createdMember);
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

              
            return _mapper.Map<IEnumerable<MemberViewModel>>(Members);
        }

        public MemberViewModel? GetMember(int MemberId)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;

           var MappedMembers = _mapper.Map<MemberViewModel>(Member);




            var ActivememberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(x => x.MemberId == MemberId && x.Status == "Active").FirstOrDefault();
            if (ActivememberShip is not null)
            {
                MappedMembers.MemberShipStartDate = ActivememberShip.CreatedAt.ToShortDateString();
                MappedMembers.MemberShipEndDate = ActivememberShip.EndDate.ToShortDateString();

                var Plan = _unitOfWork.GetRepository<Plan>().GetById(ActivememberShip.PlanId);
                MappedMembers.PlanName = Plan?.Name;

            }

            return MappedMembers;

        }

        public HealthRecordViewModel? GetMemberHealthRecordDetials(int MemberId)
        {
            var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecord is null) return null;

            return _mapper.Map<HealthRecordViewModel>(MemberHealthRecord);
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;

           return _mapper.Map<MemberToUpdateViewModel>(Member);

        }

        public bool RemoveMember(int MemberId)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();

            var Member = MemberRepo.GetById(MemberId);
            if (Member is null) return false;

            var SessionIds = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(x => x.MemberId == MemberId).Select(x => x.SessionId);

            var HasFutureSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(x => SessionIds.Contains(x.Id) && x.StartDate > DateTime.Now).Any();

            if (HasFutureSessions) return false;


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

            var emailExists = _unitOfWork.GetRepository<Member>()
                .GetAll(x => x.Email == UpdatedMember.Email && x.Id != Id);

            var PhoneExists = _unitOfWork.GetRepository<Member>()
                .GetAll(x => x.Phone == UpdatedMember.Phone && x.Id != Id);
            if (emailExists.Any() || PhoneExists.Any()) return false;
            var repo = _unitOfWork.GetRepository<Member>();

                var Member = repo.GetById(Id);
                if (Member is null) return false;

                _mapper.Map(UpdatedMember, Member);

                Member.UpdatedAt = DateTime.Now;
                repo.Update(Member) ;
                return _unitOfWork.SaveChanges() > 0;  

           
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
