using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManangementDAL.Entities;
using GymManangementDAL.Entities.Enums;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json.Linq;
using System;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerServices _trainerServices;

        public TrainerController(ITrainerServices trainerServices)
        {
            _trainerServices = trainerServices;
        }


        #region GetAlltrainers
        public ActionResult Index()
        {
            var trainers = _trainerServices.GetAllTrainers();
            return View(trainers);
        }
        #endregion

        #region get Trainer Data
        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = " Id Of Trainer can not be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var member = _trainerServices.GetTrainerDetails(id);
            if (member == null)
            {
                TempData["ErrorMessage"] = " Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }
        #endregion

        #region Create Trainer 
        public ActionResult CreateTrainer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateTrainerViewModel createTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Chech Data and missing fields");
                return View(nameof(Create), createTrainer);
            }
            bool Result = _trainerServices.CreateTrainer(createTrainer);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfullt";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed to Create , Check Phone and Email";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit Trainer

        public IActionResult TrainerEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer cannot be 0 or negative";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerServices.GetTrainerToUpdate(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TrainerId = id; 
            return View(trainer);
        }

        [HttpPost]
        public IActionResult TrainerEdit([FromRoute] int id, TrainerToUpdateViewModel updatedTrainer)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TrainerId = id;
                return View(updatedTrainer);
            }

            bool result = _trainerServices.UpdateTrainerDetails(updatedTrainer, id);

            if (result)
                TempData["SuccessMessage"] = "Trainer updated successfully";
            else
                TempData["ErrorMessage"] = "Trainer failed to update";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete Trainer

        public IActionResult DeleteTrainer(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Trainer Id cannot be 0 or negative";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerServices.GetTrainerToUpdate(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TrainerId = id;
            ViewBag.TrainerName = trainer.Name;
            return View("DeleteTrainer"); 
        }

        [HttpPost]
        public IActionResult DeleteTrainerConfirmed(int id)
        {
            if (_trainerServices.RemoveTrainer(id))
                TempData["SuccessMessage"] = "Trainer deleted successfully";
            else
                TempData["ErrorMessage"] = "Cannot delete trainer (has active sessions?)";

            return RedirectToAction(nameof(Index));
        }

        #endregion


    }
}
