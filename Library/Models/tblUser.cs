//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Library.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblUser
    {
        public tblUser()
        {
            this.tblReserves = new HashSet<tblReserve>();
        }
    
        public int Id { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public Nullable<int> Age { get; set; }
        public string Address { get; set; }
        public string Phone_Number { get; set; }
        public string Admin { get; set; }
    
        public virtual ICollection<tblReserve> tblReserves { get; set; }
    }
}
