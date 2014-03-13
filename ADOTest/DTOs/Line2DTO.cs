using GenericMapper.Classes;

namespace ADOTest.DTOs
{
    public class Line2DTO
    {
        [Map("Id")]
        public int LineId { get; set; }

        [Map("IdProducto")]
        public int ProductId { get; set; }

        [Map("Cantidad")]
        public double Count { get; set; }

        [Map("PRODUCTO.Descripcion", nameRelation: "LineaProducto")]
        public string Descripcion { get; set; }
    }
}