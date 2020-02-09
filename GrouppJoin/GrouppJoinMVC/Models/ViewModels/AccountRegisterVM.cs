using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroupJoinMVC.Models.ViewModels
{
    public class AccountRegisterVM
    {
        [Required(ErrorMessage = "Du måste uppge ett användarnamn")]
        [EmailAddress]
        public string Username { get; set; }

        [Required(ErrorMessage = "Du måste uppge ett förnamn")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string PasswordRepeat { get; set; }
    }
}
