//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ESS
{
    using System;
    using System.Collections.Generic;
    
    public partial class sec_user
    {
        public sec_user()
        {
            this.Encrypt_master = new HashSet<Encrypt_master>();
            this.Project_Master = new HashSet<Project_Master>();
            this.Sec_user_log = new HashSet<Sec_user_log>();
        }
    
        public int User_ID { get; set; }
        public string User_Name { get; set; }
        public string Full_Name { get; set; }
        public string Email_Id { get; set; }
        public string Mobile_No { get; set; }
        public string Pwd_Txt { get; set; }
        public string User_Type { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
    
        public virtual ICollection<Encrypt_master> Encrypt_master { get; set; }
        public virtual ICollection<Project_Master> Project_Master { get; set; }
        public virtual ICollection<Sec_user_log> Sec_user_log { get; set; }
    }
}
