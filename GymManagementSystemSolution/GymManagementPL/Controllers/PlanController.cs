using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]

    public class PlanController : Controller
    {
        private readonly IPlanServices _planServices;

        public PlanController(IPlanServices planServices)
        {
            _planServices = planServices;
        }

        #region Get ALL Plans
        public ActionResult Index()
        {
            var Plans = _planServices.GetAllPlans();
            return View(Plans);
        }

        #endregion

        #region Details
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = " Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planServices.GetPlanById(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = " Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        #endregion

        #region Edit 
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = " Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planServices.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = " Plan Not be Updated";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        [HttpPost]
        public ActionResult Edit(int id, UpdatePlanViewModel updatedPlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", " Check Data Validation");
                return View(updatedPlan);
            }
            var result = _planServices.UpdatePlan(id, updatedPlan);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan Updated";
            } 
            else
            {
                TempData["ErrorMessage"] = " Plan is Failed to Updated";

            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region activate
        public ActionResult Activate(int id)
        {
            var result = _planServices.ToggleStatus(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan Status Changed";
            }
            else
            {
                TempData["ErrorMessage"] = " Plan Status Failed to Change";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
