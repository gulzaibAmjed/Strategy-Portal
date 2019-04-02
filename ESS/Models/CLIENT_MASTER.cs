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
    
    public partial class CLIENT_MASTER
    {
        public CLIENT_MASTER()
        {
            this.DEVICE_MASTER = new HashSet<DEVICE_MASTER>();
        }
    
        public int CLIENT_ID { get; set; }
        public string CLIENT_NAME { get; set; }
        public string CLIENT_URL { get; set; }
        public string SERVER_NAME { get; set; }
        public string DATABASE_NAME { get; set; }
        public string DB_USER_NAME { get; set; }
        public string DB_PWD_ENCRYPT { get; set; }
        public string CONNECTION_STR { get; set; }
        public string AUTH_CODE { get; set; }
        public Nullable<decimal> CREATE_USER_ID { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<decimal> UPDATE_USER_ID { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
    
        public virtual SEC_USER SEC_USER { get; set; }
        public virtual SEC_USER SEC_USER1 { get; set; }
        public virtual ICollection<DEVICE_MASTER> DEVICE_MASTER { get; set; }
    }
}
