using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberShipViewModels;
using GymManangementDAL.Entities;
using GymManangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberShipService : IMemberShipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberShipService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<MemberShipViewModel> GetAllActiveMemberShips()
        {
            var memberships = _unitOfWork.GetRepository<MemberShip>()
                .GetAll(x => x.EndDate > DateTime.Now).ToList();

            foreach (var membership in memberships)
            {
                membership.Member = _unitOfWork.GetRepository<Member>().GetById(membership.MemberId);
                membership.Plan = _unitOfWork.GetRepository<Plan>().GetById(membership.PlanId);
            }
            return _mapper.Map<IEnumerable<MemberShipViewModel>>(memberships);

        }
        public bool CreateMemberShip(int memberId, int planId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null)
            {
                return false;
            }

            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null)
            {
                return false;
            }
            if (!plan.IsActive) return false;
                

            var hasActive = _unitOfWork.GetRepository<MemberShip>()
                .GetAll(x => x.MemberId == memberId && x.EndDate > DateTime.Now)
                .Any();
            if (hasActive)
            {
                return false;
            }

          

            var membership = new MemberShip
            {
                MemberId = memberId,
                PlanId = planId,
                CreatedAt = DateTime.Now,
                EndDate = DateTime.Now.AddDays(plan.DurationDays)
            };

            _unitOfWork.GetRepository<MemberShip>().Add(membership);
            return _unitOfWork.SaveChanges() > 0;

        }

        public bool DeleteMemberShip(int memberId , int planId)
        {
            var membership = _unitOfWork.GetRepository<MemberShip>()
                .GetAll(ms => ms.MemberId == memberId && ms.PlanId == planId).FirstOrDefault();

            if (membership == null)
                return false;

            if (membership.EndDate <= DateTime.Now)
                return false;

            _unitOfWork.GetRepository<MemberShip>().Delete(membership);
            return _unitOfWork.SaveChanges() > 0;

        }
    }
}
