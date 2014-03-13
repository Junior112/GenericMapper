﻿using GenericMapper.Classes;

namespace NHibernateTest.DTOs
{
    public class LineDTO
    {
        [Map("Id")]
        public int LineId { get; set; }

        [Map("IdProducto")]
        public int ProductId { get; set; }
        
        [Map("Cantidad")]
        public float Count { get; set; }
    }
}
