using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToRoute("Trainers" , new {action = "GetTrainers"});  
        }

        public ActionResult GetMembers()
        {
            return View();
        }

        public ActionResult CreateMember()
        {
            return View();
        }
    }
}
