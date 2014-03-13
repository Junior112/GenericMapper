using GenericMapper.Classes;

namespace EntityFrameWorkTest.DTOs
{
    public class LineDTO
    {
        [Map("Id")]
        public int LineId { get; set; }

        [Map("IdProducto")]
        public int ProductId { get; set; }
        
        [Map("Cantidad")]
        public double Count { get; set; }
    }
}
