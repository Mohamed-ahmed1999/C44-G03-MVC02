using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberShipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberShipsController : Controller
    {
        private readonly IMemberShipService _memberShipService;
        private readonly IMemberServices _memberServices;
        private readonly IPlanServices _planServices;

        public MemberShipsController(IMemberShipService memberShipService , IMemberServices memberServices , IPlanServices planServices)
        {
            _memberShipService = memberShipService;
            _memberServices = memberServices;
            _planServices = planServices;
        }

        #region GetAllMembers
        public ActionResult Index()
        {
            var memberships = _memberShipService.GetAllActiveMemberShips();
            return View(memberships);
        }
        #endregion

        #region Create 
        [HttpGet]
        public ActionResult Create()
        {
            var members = _memberServices.GetAllMembers();
            ViewBag.Members = new SelectList(members , "Id", "Name");

            var plans = _planServices.GetAllPlans();
            ViewBag.Plans = new SelectList(plans, "Id", "Name" );

            return View();
        }


        [HttpPost]
        public ActionResult Create(CreateMemberShipViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields.";
                return RedirectToAction(nameof(Create));
            }

            var result = _memberShipService.CreateMemberShip(model.MemberId, model.PlanId);

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to create membership. Please check business rules.";
                return RedirectToAction(nameof(Create));
            }

            TempData["SuccessMessage"] = "Membership created successfully.";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Cancel membership

        [HttpPost]
        public ActionResult Cancel(int memberId, int planId)
        {
            var result = _memberShipService.DeleteMemberShip(memberId, planId);
            if (result)
                TempData["SuccessMessage"] = "Membership cancelled successfully.";
            else
                TempData["ErrorMessage"] = "Failed to cancel membership.";

            return RedirectToAction(nameof(Index));

        }

        #endregion
    }
}
