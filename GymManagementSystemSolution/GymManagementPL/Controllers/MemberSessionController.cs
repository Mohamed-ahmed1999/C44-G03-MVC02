using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberSessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberSessionController : Controller
    {
        private readonly IMemberSessionService _memberSessionService;
        private readonly ISessionService _sessionService;
        private readonly IMemberServices _memberService;

        public MemberSessionController(IMemberSessionService memberSessionService,
                                       ISessionService sessionService,
                                       IMemberServices memberService)
        {
            _memberSessionService = memberSessionService;
            _sessionService = sessionService;
            _memberService = memberService;
        }

        public ActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();
            return View(sessions);
        }

        #region GetMembersForUpcomingSession
        public ActionResult GetMembersForUpcomingSession(int sessionId)
        {
            ViewBag.SessionId = sessionId;
            var result = _memberSessionService.GetMembersForUpcomingSession(sessionId);
            return View(result);
        }

        #endregion


        #region GetMembersForOngoingSessions
        public ActionResult GetMembersForOngoingSessions(int sessionId)
        {
            ViewBag.SessionId = sessionId;
            var result = _memberSessionService.GetMembersForOngoingSession(sessionId);
            return View(result);
        }

        #endregion

        #region create 

        [HttpGet]
        public ActionResult Create(int sessionId)
        {
            var member = _memberService.GetAllMembers();
            ViewBag.Members = new SelectList(member, "Id","Name" );

            ViewBag.SessionId = sessionId;
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateMemberSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please select valid member and session.";
                return View(model);
            }

            var created = _memberSessionService.CreateMemberSession(model);

            if (created)
            {
                TempData["SuccessMessage"] = "Booking created successfully.";
                return RedirectToAction("GetMembersForUpcomingSession", new { sessionId = model.SessionId });
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create booking. Please check business rules.";
                return RedirectToAction("GetMembersForUpcomingSession", new { sessionId = model.SessionId });
            }
        }
        #endregion


        #region cancel
        [HttpPost]
        public ActionResult Cancel(int memberId, int sessionId)
        {
            var success = _memberSessionService.CancelMemberSession(memberId, sessionId);

            if (success)
                TempData["SuccessMessage"] = "Booking cancelled successfully.";
            else
                TempData["ErrorMessage"] = "Failed to cancel booking.";

            return RedirectToAction("GetMembersForUpcomingSession", new { sessionId });
        }

        #endregion

        #region Attend
        [HttpPost]
        public ActionResult MarkAttendance(int memberId, int sessionId)
        {
            try
            {
                var success = _memberSessionService.MarkAttendance(memberId, sessionId);
                if (success)
                    TempData["SuccessMessage"] = "Attendance marked successfully!";
                else
                    TempData["ErrorMessage"] = "Failed to mark attendance.";

                return RedirectToAction(nameof(GetMembersForOngoingSessions), new { sessionId });
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while marking attendance.";
                return RedirectToAction(nameof(GetMembersForOngoingSessions), new { sessionId });
            }
        }

        #endregion
    }
}
