using System;
using Financial_Portal.Enums;
using System.ComponentModel.DataAnnotations;

namespace Financial_Portal.Models
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

    public class ConfigureHouseViewModel
    {
        //hidden house Id
        public int HouseholdId { get; set; }


        //bank account stuff
        [Required]
        public string BankName { get; set; }
        [Required]
        public AccountTypes AccountType { get; set; }
        [Required, Range(0.01, 9999999.99, ErrorMessage = "Enter a number between 0.01 to 9999999.99")]
        public double StartBal { get; set; }
        [Required, Range(0.01, 9999999.99, ErrorMessage = "Enter a number between 0.01 to 9999999.99")]
        public double LowBalanceLevel { get; set; }


        //bucket stuff
        [Required]
        public string BucketName { get; set; }


        //bucket item stuff
        [Required]
        public string ItemName { get; set; }
        [Required, Range(0.01, 9999999.99, ErrorMessage = "Enter a number between 0.01 to 9999999.99")]
        public double ItemTargetAmount { get; set; }

    }

}