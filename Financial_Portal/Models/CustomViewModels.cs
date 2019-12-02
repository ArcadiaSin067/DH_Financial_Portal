using System;
using System.ComponentModel.DataAnnotations;

namespace Financial_Portal.Models
{
    public class CustomViewModels
    {
        public class JoinInvitationViewModel
        {
            public int Id { get; set; }
            public Guid Code { get; set; }
            public int HouseholdId { get; set; }
            public string Email { get; set; }
        }

        public class AcceptInvitationViewModel : RegisterViewModel
        {
            public int Id { get; set; }
            public Guid Code { get; set; }
            public int HouseholdId { get; set; }
        }

        public class EditProfileViewModel
        {
            [Required]
            [StringLength(35, ErrorMessage = "{0} must be between {2} and {1} characters long.", MinimumLength = 2)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(35, ErrorMessage = "{0} must be between {2} and {1} characters long.", MinimumLength = 2)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Avatar")]
            public string AvatarPath { get; set; }
        }

    }
}