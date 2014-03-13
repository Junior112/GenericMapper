using System.Collections.Generic;

namespace NHibernateTest.Entities
{
    
    public class PRODUCTO 
    {
        public PRODUCTO() 
        {
			ALBARANXLINEAS = new List<ALBARAN_X_LINEAS>();
			FACTURASXLINEAS = new List<FACTURAS_X_LINEAS>();
			LINEA = new List<LINEA>();
        }
        public virtual int Id { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual float Costo { get; set; }
        public virtual IList<ALBARAN_X_LINEAS> ALBARANXLINEAS { get; set; }
        public virtual IList<FACTURAS_X_LINEAS> FACTURASXLINEAS { get; set; }
        public virtual IList<LINEA> LINEA { get; set; }
    }
}
