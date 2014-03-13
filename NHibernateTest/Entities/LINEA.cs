using System.Collections.Generic;

namespace NHibernateTest.Entities
{
    
    public class LINEA 
    {
        public LINEA() 
        {
			ALBARANLINEA = new List<ALBARAN_LINEA>();
            FACTURALINEA = new List<FACTURA_LINEA>();
        }
        public virtual int Id { get; set; }
        public virtual PRODUCTO PRODUCTO { get; set; }
        public virtual float Cantidad { get; set; }
        public virtual int IdProducto { get; set; }

        public virtual IList<ALBARAN_LINEA> ALBARANLINEA { get; set; }
        public virtual IList<FACTURA_LINEA> FACTURALINEA { get; set; }
    }
}
