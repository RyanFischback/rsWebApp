//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace deadinside.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class player
    {
        public int id { get; set; }
        [DisplayName("Player Name")]
        public string playername { get; set; }
        public string playerpassword { get; set; }
        public string playerrole { get; set; }
    }
}