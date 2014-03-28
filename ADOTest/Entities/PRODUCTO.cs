using System.Collections.Generic;

namespace ADOTest.Entities
{
    
    public class PRODUCTO
    {
        public PRODUCTO() 
        {
			LINEA = new List<LINEA>();
        }

        public virtual string Descripcion { get; set; }
        public virtual double Costo { get; set; }

        [GenericMapper.Classes.Map("LINEA", true, nameRelation: "ProductoLinea")]
        public virtual List<LINEA> LINEA { get; set; }
    }
}
