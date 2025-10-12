using GymManangementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.TrainerViewModels
{
    public class TrainerToUpdateViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 100 characters")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone must be a valid Egyptian number")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Building Number is required")]
        [Range(1, 9000, ErrorMessage = "Building number must be between 1 and 9000")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street is required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 30 characters")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City must be between 2 and 30 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can contain only letters and spaces")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Specialization is required")]
        public Specialties Specialization { get; set; }
    }
}
