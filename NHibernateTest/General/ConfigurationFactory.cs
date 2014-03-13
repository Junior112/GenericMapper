using System.Configuration;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NHibernateTest.Entities;
using Configuration = NHibernate.Cfg.Configuration;

namespace NHibernateTest.General
{

    /// <summary>
    /// Class to configure the nhibernate connection and schema
    /// </summary>
    public  class ConfigurationFactory
    {
        private ConnectionStringSettings ConnectionString {
            get
            {
                return ConfigurationManager.ConnectionStrings["Test"];
            }
        }

        private Configuration DBConfiguration { get;  set; }

        private HbmMapping Mappings { get;  set; }

        /// <summary>
        /// The session factory used by all repositories
        /// </summary>
        public  ISessionFactory SessionFactory 
        {
            get 
            {
                return ConfigurationFactory.SessionFactoryStatic;
            }
        }

        /// <summary>
        /// This is static since its the same info, this contains info related to the DB and schema
        /// </summary>
        private static ISessionFactory SessionFactoryStatic { get; set; }

        /// <summary>
        /// default ctor
        /// </summary>
        public ConfigurationFactory(bool initialize)
        {
            if (initialize)
            {
                SetupDataContext();
            }
        }

        /// <summary>
        /// Setup the DB details
        /// </summary>
        public void SetupDataContext()
        {
            if (ConfigurationFactory.SessionFactoryStatic == null)
            {
                this.DBConfiguration = CreateConfiguration();
                this.Mappings = GenerateMappings();
            
                SchemaMetadataUpdater.QuoteTableAndColumns(this.DBConfiguration);
                ConfigurationFactory.SessionFactoryStatic = this.DBConfiguration.BuildSessionFactory();
            }
            //ValidateSchema(); //uncomment this when your DB matches exactly the same as the schema in code
        }

        /// <summary>
        /// Creates a new configuration for nhibernate
        /// </summary>
        private  Configuration CreateConfiguration()
        {
            Configuration ejfConfig = null;
            ejfConfig = ConfigureNHibernate();
            var mappings = GenerateMappings();
            ejfConfig.AddDeserializedMapping(mappings, "NHibernateTest");
            return ejfConfig;
        }

        /// <summary>
        /// Configuration of model mapper
        /// </summary>
        /// <returns>HbmMapping class is equal to hbm.xml (mapping file)</returns>
        private HbmMapping GenerateMappings()
        {
            var typeDomainClass = typeof(ALBARAN);
            var mapper = new ModelMapper();
            
            //Get all types are in assembly of 'type' variable
            var typesOfDomains = Assembly.GetAssembly(typeDomainClass).GetExportedTypes();
            //Insert all types of class domains for create configuration
            mapper.AddMappings(typesOfDomains);
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            return mapping;
        }

        /// <summary>
        /// Configuration nhibernate and data base
        /// </summary>
        /// <returns></returns>
        private Configuration ConfigureNHibernate()
        {
            var configure = new Configuration();
            configure.SessionFactoryName("SQL");
            
            configure.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2012Dialect>();
                db.Driver<NHibernate.Driver.SqlClientDriver>();
                db.ConnectionString = this.ConnectionString.ConnectionString;
                db.Timeout = 10;
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                db.IsolationLevel = System.Data.IsolationLevel.ReadCommitted;

            });
            
            return configure;
        }


        /// <summary>
        /// validates that the DB schema and the schema in code are the same
        /// </summary>
        private void ValidateSchema()
        {
            var schemaValidator = new SchemaValidator(this.DBConfiguration);
            schemaValidator.Validate();
        }
    }
}
