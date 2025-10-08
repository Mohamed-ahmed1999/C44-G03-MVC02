using GymManangementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    internal class CreateMemberViewModel
    {
        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50 , MinimumLength = 2 , ErrorMessage = "Name Must Be Between 2 And 50 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$" , ErrorMessage ="Name Can Contain Only Letters And Spaces")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage ="Invalid email Format")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100,MinimumLength =5 , ErrorMessage ="Email Must Be Between 5 and 100 Char")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone is Required")]
        [Phone(ErrorMessage ="Invalid Phone Format")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$" , ErrorMessage ="Phone Number Must Be Valid Egyption PhoneNumber")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "DateOfBirth is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is Required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "BuildingNumber is Required")]
        [Range(1,9000 , ErrorMessage ="Building Number Must Be Between 1 and 9000")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street is Required")]
        [StringLength(30 ,MinimumLength = 2 ,ErrorMessage ="Street Must be Between 2 and 30 Chars")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City Must be Between 2 and 30 Chars")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Can Contain Only Letters And Spaces")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage ="Healt Record is Required")]
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = null!;
    }
}
