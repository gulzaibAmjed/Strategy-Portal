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
    
    public partial class EMAIL_TEMPLATE
    {
        public int TEMPLATE_ID { get; set; }
        public Nullable<byte> TEMPLATE_CODE { get; set; }
        public Nullable<int> LANGUAGE_ID { get; set; }
        public string MESSAGE_SENDER { get; set; }
        public string MESSAGE_TITLE { get; set; }
        public string MESSAGE_BODY { get; set; }
        public string MESSAGE_SIGNATURE { get; set; }
    
        public virtual SYS_LANGUAGE SYS_LANGUAGE { get; set; }
    }
}
