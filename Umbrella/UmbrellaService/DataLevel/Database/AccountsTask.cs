//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UmbrellaService.DataLevel.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class AccountsTask
    {
        public int Id { get; set; }
        public Nullable<int> Account { get; set; }
        public Nullable<int> Task { get; set; }
    
        public virtual Account Account1 { get; set; }
        public virtual Task Task1 { get; set; }
    }
}
