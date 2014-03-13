namespace ADOTest.Entities
{
    
    public class ALBARAN_LINEA {
        public virtual int IdAlbaran { get; set; }
        public virtual int IdLinea { get; set; }
        public virtual ALBARAN ALBARAN { get; set; }
        public virtual LINEA LINEA { get; set; }
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj) {
			if (obj == null) return false;
			var t = obj as ALBARAN_LINEA;
			if (t == null) return false;
			if (IdAlbaran == t.IdAlbaran
			 && IdLinea == t.IdLinea)
				return true;

			return false;
        }
        public override int GetHashCode() {
			int hash = 13;
			hash += IdAlbaran.GetHashCode();
			hash += IdLinea.GetHashCode();

			return hash;
        }
        #endregion
    }
}
