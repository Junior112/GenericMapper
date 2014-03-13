namespace ADOTest.Entities
{
    public class FACTURAS_X_LINEAS 
    {
        public virtual int IdLinea { get; set; }
        public virtual FACTURAS FACTURAS { get; set; }
        public virtual PRODUCTO PRODUCTO { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual float Cantidad { get; set; }
        public virtual float Precio { get; set; }
    }
}
