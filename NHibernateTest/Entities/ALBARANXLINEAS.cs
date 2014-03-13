namespace NHibernateTest.Entities
{
    
    public class ALBARAN_X_LINEAS {
        public virtual int IdLinea { get; set; }
        public virtual ALBARANES ALBARANES { get; set; }
        public virtual PRODUCTO PRODUCTO { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual float Cantidad { get; set; }
        public virtual float Precio { get; set; }
    }
}
