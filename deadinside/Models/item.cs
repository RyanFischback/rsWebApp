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
    using System.ComponentModel.DataAnnotations;

    public partial class item
    {
        public int id { get; set; }
        [DisplayName("Item Name")]
        public string itemname { get; set; }
        [DisplayName("Item Description")]
        public string itemdesc { get; set; }
        public string itemURL { get; set; }
        [DisplayName("Item Value")]
        [DataType(DataType.Currency)]
        public int itemValue { get; set; }
    }
}