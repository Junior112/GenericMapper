//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityFrameWorkTest
{
    using System;
    using System.Collections.Generic;
    
    public partial class FACTURAS
    {
        public FACTURAS()
        {
            this.FACTURAS_X_LINEAS = new HashSet<FACTURAS_X_LINEAS>();
        }
    
        public int IdFactura { get; set; }
    
        public virtual ICollection<FACTURAS_X_LINEAS> FACTURAS_X_LINEAS { get; set; }
    }
}