using System;
using System.Configuration;
using System.Collections.Generic;
using EntityFrameWorkTest.DTOs;
using GenericMapper.Interfaces;
using NUnit.Framework;
using GenericMapper.Extensions;
using System.Linq;
using System.Data.Common;

namespace EntityFrameWorkTest.Test
{
    [TestFixture]
    public class ProductTest
    {
        // TODO: Create a base class for all the integration fixtures
        [SetUp]
        public void TestSetUp() 
        {
            var executeIntegrationTests = true;
            try{
                var builder = new DbConnectionStringBuilder
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString
                };
            } catch(ArgumentException ex){
                Console.Error.WriteLine(ex.Message);
                executeIntegrationTests = false; // Invalid connection string
            }
            
             if (executeIntegrationTests)
             {
                 Assert.Ignore( "Local SQL Express is not setup.  Omitting integration fixture." );
             }
        }
        
        [Test]
        public void Map_A_Product()
        {
            IEnumerable<IDto> dtos;
            using (var db = new TestEntities())
            {
                var list = db.Set<PRODUCTO>().Take(100).ToList().ConvertListToDto<ProductDTO>();
                dtos = list.ToList();
            }
            Assert.Greater(dtos.Count(), 0);
        }
    }
}
