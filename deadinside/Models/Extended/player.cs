using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace deadinside.Models
{
    //add validation
    [MetadataType(typeof(playermetadata))]
    public partial class player
    {
    }

    public class playermetadata
    {
        [Display(Name = "Player Name")]
        [Required(AllowEmptyStrings =false, ErrorMessage = "Please enter a Player Name")]
        public string playername { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        [MinLength(3, ErrorMessage = "Wow real secure, try again (4 or more characters)")]
        public string playerpassword { get; set; }
    }
}