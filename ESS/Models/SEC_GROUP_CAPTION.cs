//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ESS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SEC_GROUP_CAPTION
    {
        public byte SEC_GROUP_ID { get; set; }
        public int LANGUAGE_ID { get; set; }
        public string SEC_GROUP_DESC { get; set; }
    
        public virtual SEC_GROUP SEC_GROUP { get; set; }
        public virtual SYS_LANGUAGE SYS_LANGUAGE { get; set; }
    }
}
