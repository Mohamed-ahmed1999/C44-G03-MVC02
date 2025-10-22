using GymManagementBLL.Services.Interface;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService) 
        {
            _sessionService = sessionService;
        }

        #region GetAll Session
        public ActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();
            return View(sessions);
        }

        #endregion

        #region Session Details
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = " Id Of Session can not be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session == null)
            {
                TempData["ErrorMessage"] = " Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        #endregion

        #region Create Session 

        public ActionResult Create()
        {
            LoadDropDownsForTrainers();
            LoadDropDownsForCategories();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsForTrainers();
                LoadDropDownsForCategories();
                return View(createSession);
            }
            var Result = _sessionService.CreateSession(createSession);
            if (Result)
            {
                TempData["SuccessMessage"] = " Session Created Successfully";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = " Failed to Create Session";
                LoadDropDownsForTrainers();
                LoadDropDownsForCategories();
                return View(createSession);
            }
        }
        #endregion

        #region Helper Method
        private void LoadDropDownsForTrainers()
        {
            
            var Trainers = _sessionService.GetTrainerForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }

        private void LoadDropDownsForCategories()
        {
            var Categories = _sessionService.GetCategoryForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }
        #endregion

        #region Edit
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = " Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionToUpdate(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = " Session Can Not Be Updated";
                return RedirectToAction(nameof(Index));
            }
            LoadDropDownsForTrainers();
            return View(session);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute]int id , UpdateSessionViewModel updateSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsForTrainers();
                return View(updateSession);
            }
            var Result = _sessionService.UpdateSession(updateSession, id);
            if (Result)
            {
                TempData["SuccessMessage"] = " Session Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = " Failed to Update Session";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete 
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = " Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = " Failed to Delete Session";
                return RedirectToAction(nameof(Index));

            }
            
            ViewBag.SessionId = session.Id;
            return View(session);
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var Result = _sessionService.RemoveSession(id);
            if (Result)
            {
                TempData["SuccessMessage"] = " Session Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = " Failed to Delete Session";
            }
            return RedirectToAction(nameof(Index));
        }


        #endregion

    }
}
