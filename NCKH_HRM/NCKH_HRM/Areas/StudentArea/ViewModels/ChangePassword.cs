﻿using System.ComponentModel.DataAnnotations;

namespace NCKH_HRM.Areas.StudentArea.ViewModels
{
    public class ChangePassword
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới không khớp")]
        public string ConfirmPassword { get; set; }
    }
}
