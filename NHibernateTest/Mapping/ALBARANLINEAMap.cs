using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using NHibernateTest.Entities;

namespace NHibernateTest.Mapping 
{
    public class ALBARANLINEAMap : ClassMapping<ALBARAN_LINEA> {
        
        public ALBARANLINEAMap() {
			Table("ALBARAN_LINEA");
			Schema("dbo");
			Lazy(true);
			ComposedId(compId =>
				{
					compId.Property(x => x.IdAlbaran, m => m.Column("IdAlbaran"));
					compId.Property(x => x.IdLinea, m => m.Column("IdLinea"));
				});
			ManyToOne(x => x.ALBARAN, map => { map.Column("IdAlbaran"); map.Cascade(Cascade.None); });

			ManyToOne(x => x.LINEA, map => 
			{
				map.Column("IdLinea");
				map.PropertyRef("Id");
				map.Cascade(Cascade.None);
			});

        }
    }
}
