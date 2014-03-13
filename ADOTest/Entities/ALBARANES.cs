using System.Collections.Generic;

namespace ADOTest.Entities
{
    
    public class ALBARANES 
    {
        public ALBARANES() 
        {
			ALBARANXLINEAS = new List<ALBARAN_X_LINEAS>();
        }
        public virtual int IdAlbaran { get; set; }
        public virtual string VALOR1 { get; set; }
        public virtual string VALOR2 { get; set; }
        public virtual IList<ALBARAN_X_LINEAS> ALBARANXLINEAS { get; set; }
    }
}
