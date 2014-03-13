using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using NHibernateTest.Entities;

namespace NHibernateTest.Mapping 
{
    public class PRODUCTOMap : ClassMapping<PRODUCTO> 
    {
        public PRODUCTOMap() 
        {
			Schema("dbo");
			Lazy(true);
			Id(x => x.Id, map => map.Generator(Generators.Identity));
			Property(x => x.Descripcion, map => { map.NotNullable(true); map.Length(50); });
			Property(x => x.Costo, map => { map.NotNullable(true); map.Precision(53); });
			Bag(x => x.ALBARANXLINEAS, colmap =>  { colmap.Key(x => x.Column("IdProducto")); colmap.Inverse(true); }, map => { map.OneToMany(); });
			Bag(x => x.FACTURASXLINEAS, colmap =>  { colmap.Key(x => x.Column("IdProducto")); colmap.Inverse(true); }, map => { map.OneToMany(); });
			Bag(x => x.LINEA, colmap =>  { colmap.Key(x => x.Column("IdProducto")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
