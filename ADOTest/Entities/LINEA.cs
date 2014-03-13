using System.Collections.Generic;

namespace ADOTest.Entities
{
    
    public class LINEA 
    {
        public LINEA() 
        {
			ALBARANLINEA = new List<ALBARAN_LINEA>();
            FACTURALINEA = new List<FACTURA_LINEA>();
        }
        public virtual int Id { get; set; }
        [GenericMapper.Classes.Map("PRODUCTO", nameRelation: "LineaProducto")]
        public virtual PRODUCTO PRODUCTO { get; set; }
        public virtual double Cantidad { get; set; }
        public virtual int IdProducto { get; set; }

        public virtual List<ALBARAN_LINEA> ALBARANLINEA { get; set; }
        public virtual List<FACTURA_LINEA> FACTURALINEA { get; set; }
    }
}
