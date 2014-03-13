using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using NHibernateTest.Entities;

namespace NHibernateTest.Mapping {
    
    
    public class FACTURASXLINEASMap : ClassMapping<FACTURAS_X_LINEAS> {
        
        public FACTURASXLINEASMap() {
			Table("FACTURAS_X_LINEAS");
			Schema("dbo");
			Lazy(true);
			Id(x => x.IdLinea, map => map.Generator(Generators.Identity));
			Property(x => x.Descripcion, map => { map.NotNullable(true); map.Length(50); });
			Property(x => x.Cantidad, map => { map.NotNullable(true); map.Precision(53); });
			Property(x => x.Precio, map => { map.NotNullable(true); map.Precision(53); });
			ManyToOne(x => x.FACTURAS, map => { map.Column("IdFactura"); map.Cascade(Cascade.None); });

			ManyToOne(x => x.PRODUCTO, map => { map.Column("IdProducto"); map.Cascade(Cascade.None); });

        }
    }
}
