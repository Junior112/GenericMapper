using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using NHibernateTest.Entities;

namespace NHibernateTest.Mapping 
{    
    public class FACTURAMap : ClassMapping<FACTURA> 
    {
        public FACTURAMap() 
        {
			Schema("dbo");
			Lazy(true);
			Id(x => x.Id, map => map.Generator(Generators.Identity));
			Bag(x => x.FACTURALINEA, colmap =>  { colmap.Key(x => x.Column("IdFactura")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
