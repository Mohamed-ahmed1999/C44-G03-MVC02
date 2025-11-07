using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberShipViewModels
{
    public class CreateMemberShipViewModel
    {
        [Required(ErrorMessage = "MemberId is Required")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "PlanId is Required")]
        public int PlanId { get; set; }
    }
}

