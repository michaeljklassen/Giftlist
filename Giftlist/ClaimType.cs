//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Giftlist
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClaimType
    {
        public ClaimType()
        {
            this.ItemClaims = new HashSet<ItemClaim>();
        }
    
        public int ClaimTypeId { get; set; }
        public string Title { get; set; }
    
        public virtual ICollection<ItemClaim> ItemClaims { get; set; }
    }
}
