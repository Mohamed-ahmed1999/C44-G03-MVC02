using GymManangementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.TrainerViewModels
{
    internal class CreateTrainerViewModel
    {
        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name Must Be Between 2 And 50 Characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name Can Contain Only Letters And Spaces")]
        public string Name { get; set; } = null!;



        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must Be Between 5 And 100 Characters")]
        public string Email { get; set; } = null!;



        [Required(ErrorMessage = "Phone is Required")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone Number Must Be A Valid Egyptian Number")]
        public string Phone { get; set; } = null!;



        [Required(ErrorMessage = "DateOfBirth is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }



        [Required(ErrorMessage = "Gender is Required")]
        public Gender Gender { get; set; }



        [Required(ErrorMessage = "BuildingNumber is Required")]
        [Range(1, 9000, ErrorMessage = "Building Number Must Be Between 1 and 9000")]
        public int BuildingNumber { get; set; }



        [Required(ErrorMessage = "Street is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must be Between 2 and 30 Chars")]
        public string Street { get; set; } = null!;



        [Required(ErrorMessage = "City is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City Must be Between 2 and 30 Chars")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Can Contain Only Letters And Spaces")]
        public string City { get; set; } = null!;


        [Required(ErrorMessage = "Specialization is Required")]
        public Specialties Specialization { get; set; } 
    }
}
