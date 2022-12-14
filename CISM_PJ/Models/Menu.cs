//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CISM_PJ.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Menu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Menu()
        {
            this.Menus1 = new HashSet<Menu>();
            this.RolePermissions = new HashSet<RolePermission>();
        }
    
        public short menuid { get; set; }
        public string menu_name { get; set; }
        public string area { get; set; }
        public string controller_name { get; set; }
        public string action_name { get; set; }
        public Nullable<short> parent_menuid { get; set; }
        public bool isprogram { get; set; }
        public bool is_sa_use { get; set; }
        public short sort_seq { get; set; }
        public bool isactive { get; set; }
        public string menu_icon { get; set; }
        public System.DateTime createddate { get; set; }
        public System.DateTime modifieddate { get; set; }
        public int createduser { get; set; }
        public int modifieduser { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Menu> Menus1 { get; set; }
        public virtual Menu Menu1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
