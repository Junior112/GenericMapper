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
        
        [GenericMapper.Classes.Map("PRODUCTO", true, nameRelation: "LineaProducto")]
        public virtual PRODUCTO PRODUCTO { get; set; }

        [GenericMapper.Classes.Map("Cantidad")]
        public virtual double Cantidad2 { get; set; }
        
        public virtual int IdProducto { get; set; }

        [GenericMapper.Classes.Map("ALBARANLINEA", true, nameRelation: "LineaLineaAlbaran")]
        public virtual List<ALBARAN_LINEA> ALBARANLINEA { get; set; }
        
        public virtual List<FACTURA_LINEA> FACTURALINEA { get; set; }
    }
}
