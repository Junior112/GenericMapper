using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using NHibernateTest.Entities;

namespace NHibernateTest.Mapping 
{
    public class FACTURASMap : ClassMapping<FACTURAS> 
    {
        public FACTURASMap() 
        {
			Schema("dbo");
			Lazy(true);
			Id(x => x.IdFactura, map => map.Generator(Generators.Identity));
			Bag(x => x.FACTURASXLINEAS, colmap =>  { colmap.Key(x => x.Column("IdFactura")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
