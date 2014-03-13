namespace ADOTest.Entities
{

    public class FACTURA_LINEA
    {
        public virtual int IdFactura { get; set; }
        public virtual int IdLinea { get; set; }
        public virtual FACTURA FACTURA { get; set; }
        public virtual LINEA LINEA { get; set; }
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj) {
			if (obj == null) return false;
            var t = obj as FACTURA_LINEA;
			if (t == null) return false;
			if (IdFactura == t.IdFactura
			 && IdLinea == t.IdLinea)
				return true;

			return false;
        }
        public override int GetHashCode() {
			int hash = 13;
			hash += IdFactura.GetHashCode();
			hash += IdLinea.GetHashCode();

			return hash;
        }
        #endregion
    }
}
