using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using NHibernateTest.Entities;

namespace NHibernateTest.Mapping {
    
    
    public class ALBARANESMap : ClassMapping<ALBARANES> {
        
        public ALBARANESMap() {
			Schema("dbo");
			Lazy(true);
			Id(x => x.IdAlbaran, map => map.Generator(Generators.Identity));
			Property(x => x.VALOR1, map => { map.NotNullable(true); map.Length(50); });
			Property(x => x.VALOR2, map => { map.NotNullable(true); map.Length(50); });
			Bag(x => x.ALBARANXLINEAS, colmap =>  { colmap.Key(x => x.Column("IdAlbaran")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
