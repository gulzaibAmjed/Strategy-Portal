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
    
    public partial class SEC_LOG_INFO
    {
        public string SESSION_KEY { get; set; }
        public Nullable<decimal> USER_ID { get; set; }
        public Nullable<System.DateTime> TIME_IN { get; set; }
        public Nullable<System.DateTime> TIME_OUT { get; set; }
        public string TERMINAL_NAME { get; set; }
        public string TERMINAL_IP { get; set; }
        public Nullable<byte> SESSION_STATUS { get; set; }
    
        public virtual SEC_USER SEC_USER { get; set; }
    }
}
