using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace deadinside.Models
{
    public class PlayerLogin
    {
        [Display(Name = "Player Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Player Name")]
        public string playername { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool rememberMe { get; set; }

    }
}