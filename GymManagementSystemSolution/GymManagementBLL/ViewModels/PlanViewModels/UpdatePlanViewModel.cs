using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewModels
{
    internal class UpdatePlanViewModel
    {
        [Required(ErrorMessage ="Plan Name is Required")]
        [StringLength(50 , ErrorMessage ="plan name must be Less Than 51 Char")]
        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = "Description is Required")]
        [StringLength(50, MinimumLength = 5 ,ErrorMessage = "Description must be between 5 and 200  Char")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Durtaion Days is Required")]
        [Range(1 ,365 ,ErrorMessage ="Duration Days Must Be Between 1 and 365")]
        public int DurtaionDays { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        [Range(0.1, 10000, ErrorMessage = "Price Must Be between 0.1 and 10000")]
        public decimal Price { get; set; }
    }
}
