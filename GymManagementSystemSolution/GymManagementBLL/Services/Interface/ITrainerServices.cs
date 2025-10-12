using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interface
{
    public interface ITrainerServices
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        bool CreateTrainer(CreateTrainerViewModel createdTrainer);
        TrainerViewModel? GetTrainerDetails(int TrainerId);
        bool UpdateTrainerDetails(int Id, TrainerToUpdateViewModel UpdatedTrainer);
        bool RemoveTrainer(int TrainerId);
    }
}
