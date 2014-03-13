using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using NHibernateTest.Entities;

namespace NHibernateTest.Mapping 
{
    public class LINEAMap : ClassMapping<LINEA> 
    {
        public LINEAMap() 
        {
			Schema("dbo");
			Lazy(true);
			Id(x => x.Id, map => map.Generator(Generators.Identity));
            Property(x => x.Cantidad, map => { map.NotNullable(true); map.Precision(53); });
            Property(x => x.IdProducto, map => { map.NotNullable(true); });
			
            ManyToOne(x => x.PRODUCTO, map => { map.Column("IdProducto"); map.Cascade(Cascade.None); });

			Bag(x => x.ALBARANLINEA, colmap =>  { colmap.Key(x => x.Column("IdLinea")); colmap.Inverse(true); }, map => { map.OneToMany(); });
			Bag(x => x.FACTURALINEA, colmap =>  { colmap.Key(x => x.Column("IdLinea")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
