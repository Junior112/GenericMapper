using System.Collections.Generic;
using ADOTest.Entities;
using GenericMapper.Classes;

namespace ADOTest.DTOs
{
    public class Line3DTO
    {
        [Map("Id")]
        public int LineId { get; set; }

        [Map("IdProducto")]
        public int ProductId { get; set; }

        [Map("Cantidad")]
        public double Count { get; set; }

        [Map("LINEAS_ALBARANES", nameRelation: "LineaLineaAlbaran")]
        public List<ALBARAN_LINEA> LINEAS_ALBARANES { get; set; }
    }
}