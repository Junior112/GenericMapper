using System.Collections.Generic;

namespace ADOTest.Entities
{
    
    public class PRODUCTO
    {
        public PRODUCTO() 
        {
			ALBARANXLINEAS = new List<ALBARAN_X_LINEAS>();
			FACTURASXLINEAS = new List<FACTURAS_X_LINEAS>();
			LINEA = new List<LINEA>();
        }

        public virtual string Descripcion { get; set; }
        public virtual double Costo { get; set; }
        public virtual List<ALBARAN_X_LINEAS> ALBARANXLINEAS { get; set; }
        public virtual List<FACTURAS_X_LINEAS> FACTURASXLINEAS { get; set; }

        [GenericMapper.Classes.Map("LINEA", true, nameRelation: "ProductoLinea")]
        public virtual List<LINEA> LINEA { get; set; }
    }
}
