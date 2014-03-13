using System.Collections.Generic;

namespace NHibernateTest.Entities
{
    public class FACTURA 
    {
        public FACTURA() 
        {
			FACTURALINEA = new List<FACTURA_LINEA>();
        }
        public virtual int Id { get; set; }
        public virtual IList<FACTURA_LINEA> FACTURALINEA { get; set; }
    }
}
