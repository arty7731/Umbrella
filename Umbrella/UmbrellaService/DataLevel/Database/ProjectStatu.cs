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
    
    public partial class ProjectStatu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Project { get; set; }
    
        public virtual Project Project1 { get; set; }
    }
}
