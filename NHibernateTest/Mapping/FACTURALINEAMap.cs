using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using NHibernateTest.Entities;

namespace NHibernateTest.Mapping 
{
    public class FACTURALINEAMap : ClassMapping<FACTURA_LINEA> 
    {    
        public FACTURALINEAMap() 
        {
			Table("FACTURA_LINEA");
			Schema("dbo");
			Lazy(true);
			ComposedId(compId =>
				{
					compId.Property(x => x.IdFactura, m => m.Column("IdFactura"));
					compId.Property(x => x.IdLinea, m => m.Column("IdLinea"));
				});
			ManyToOne(x => x.FACTURA, map => { map.Column("IdFactura"); map.Cascade(Cascade.None); });

			ManyToOne(x => x.LINEA, map => 
			{
				map.Column("IdLinea");
				map.PropertyRef("Id");
				map.Cascade(Cascade.None);
			});

        }
    }
}
