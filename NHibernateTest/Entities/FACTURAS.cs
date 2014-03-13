using System.Collections.Generic;

namespace NHibernateTest.Entities 
{
    
    public class FACTURAS 
    {
        public FACTURAS() 
        {
			FACTURASXLINEAS = new List<FACTURAS_X_LINEAS>();
        }
        public virtual int IdFactura { get; set; }
        public virtual IList<FACTURAS_X_LINEAS> FACTURASXLINEAS { get; set; }
    }
}
