using GymManagementBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interface
{
    internal interface IPlanServices
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanById(int planId);

        UpdatePlanViewModel? GetPlanToUpdate(int Id);

        bool UpdatePlan(int PlanId, UpdatePlanViewModel UpdatedPlan);

        bool ToggleStatus(int PlanId);
    }
}
