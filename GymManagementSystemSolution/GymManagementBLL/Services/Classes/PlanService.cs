using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.PlanViewModels;
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
    public  class PlanService : IPlanServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (Plans is null || Plans.Any()) return [];

            return _mapper.Map<IEnumerable<PlanViewModel>>(Plans);
        }

        public PlanViewModel? GetPlanById(int planId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (Plan is null) return null;

            return _mapper.Map<PlanViewModel>(Plan);
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null || Plan.IsActive == false || HasActiveMemberShips(PlanId)) return null;

            return _mapper.Map<UpdatePlanViewModel>(Plan);
        }

        public bool UpdatePlan(int PlanId, UpdatePlanViewModel UpdatedPlan)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null || HasActiveMemberShips(PlanId)) return false;

            try
            {

                //(Plan.Description, Plan.Price, Plan.DurationDays, Plan.UpdatedAt)
                //    = (UpdatedPlan.Description, UpdatedPlan.Price, UpdatedPlan.DurtaionDays, DateTime.Now);
                _mapper.Map(UpdatedPlan, Plan);
                Plan.UpdatedAt = DateTime.Now;

                _unitOfWork.GetRepository<Plan>().Update(Plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch 
            {
                return false;
            }

        }

        public bool ToggleStatus(int PlanId)
        {
            var Repo = _unitOfWork.GetRepository<Plan>();
            var Plan = Repo.GetById(PlanId);
            if (Plan is null || HasActiveMemberShips(PlanId)) return false;
            Plan.IsActive = Plan.IsActive == true ? false : true;

            Plan.UpdatedAt = DateTime.Now;

            try 
            {
                Repo.Update(Plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch 
            {
                return false;
            }

        }
      

        #region Helper
        
        private bool HasActiveMemberShips(int planId)
        {
            var ActiveMemberShips = _unitOfWork.GetRepository<MemberShip>()
                .GetAll(X => X.PlanId == planId && X.Status == "Active");
            return ActiveMemberShips.Any();
        }
        #endregion
    }
}
