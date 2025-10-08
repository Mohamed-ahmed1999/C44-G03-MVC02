﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    internal class HealthRecordViewModel
    {
        [Required(ErrorMessage ="Height is Required")]
        [Range(0.1,300 , ErrorMessage ="Height Must Be Greater than 0 and less than 300")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "Weight is Required")]
        [Range(0.1, 500, ErrorMessage = "Weight Must Be Greater than 0 and less than 500")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "BloodType is Required")]
        [StringLength(3 , ErrorMessage = "Blood Type Must be 3 Char Or Less")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }


    }
}
