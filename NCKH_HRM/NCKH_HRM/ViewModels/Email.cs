using System.ComponentModel.DataAnnotations;

namespace NCKH_HRM.ViewModels
{
    public class Email
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}