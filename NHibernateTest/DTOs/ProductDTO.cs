using System.Collections.Generic;
using GenericMapper.Classes;
using GenericMapper.Interfaces;


namespace NHibernateTest.DTOs
{
    public class ProductDTO : IDto
    {
        // MAIN
        #region " MAIN "

        [Map("Id")]
        public int ProductId { get; set; }

        [Map("Descripcion")]
        public string Description { get; set; }

        [Map("Costo")]
        public float Cost { get; set; }

        [Map("LINEA", true)]
        public List<LineDTO> Lines { get; set; }

        #endregion

    }
}
