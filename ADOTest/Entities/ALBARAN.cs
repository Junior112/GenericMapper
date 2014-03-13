using System.Collections.Generic;

namespace ADOTest.Entities
{
    
    public class ALBARAN 
    {
        public ALBARAN() 
        {
			ALBARANLINEA = new List<ALBARAN_LINEA>();
        }
        public virtual int Id { get; set; }
        public virtual string VALOR1 { get; set; }
        public virtual string VALOR2 { get; set; }
        public virtual IList<ALBARAN_LINEA> ALBARANLINEA { get; set; }
    }
}
