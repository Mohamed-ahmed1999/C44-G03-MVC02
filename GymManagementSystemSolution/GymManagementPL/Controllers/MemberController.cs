using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberServices _memberServices;

        public MemberController(IMemberServices memberServices)
        {
            _memberServices = memberServices;
        }

        #region GetAllMembers
        public ActionResult Index()
        {
            var members = _memberServices.GetAllMembers();
            return View(members);
        }
        #endregion

        #region Get Member Data 
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = " Id Of Member can not be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberServices.GetMember(id);
            if (member == null)
            {
                TempData["ErrorMessage"] = " Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = " Id Of Member can not be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var healthRecord = _memberServices.GetMemberHealthRecordDetials(id);
            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = " healthRecord Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }
        #endregion

        #region Create Member 
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel CreatedMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid" , "Chech Data and missing fields");
                return View(nameof(Create) , CreatedMember);
            }
            bool Result = _memberServices.CreateMember(CreatedMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfullt";
            }
            else
            {
               TempData["ErrorMessage"] = "Member Failed to Create , Check Phone and Email";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region Edit Member

        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = " Id Of Member can not be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberServices.GetMemberToUpdate(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = " Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }
        [HttpPost]
        public ActionResult MemberEdit([FromRoute] int id , MemberToUpdateViewModel MemberToEdit)
        {
            if (!ModelState.IsValid)
                return View(MemberToEdit);
            var Result = _memberServices.UpdateMemberDetails(id , MemberToEdit);
            if (Result)
            {
                TempData["SuccessMessage"] = " Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = " Member Failed to Update";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Member

        public ActionResult DeleteTrainer(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = " Id Of Member can not be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberServices.GetMember(id);
            if (id <= 0)
            {
                TempData["ErrorMessage"] = " Member Not Found ";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            ViewBag.MemberName = Member.Name;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm]int id)
        {
            var Result = _memberServices.RemoveMember(id);
            if (Result)
            {
                TempData["SuccessMessage"] = " Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = " Member can Not Deleted";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
