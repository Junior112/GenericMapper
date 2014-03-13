using System.Collections.Generic;
using GenericMapper.Interfaces;
using NHibernate.Linq;
using NHibernateTest.DTOs;
using NHibernateTest.Entities;
using NHibernateTest.General;
using NUnit.Framework;
using GenericMapper.Extensions;
using System.Linq;

namespace EntityFrameWorkTest.Test
{
    [TestFixture]
    public class ProductTest
    {
        [Test]
        public void Map_A_Product()
        {
            IEnumerable<IDto> dtos;
            var nhConfiguration = new ConfigurationFactory(true);
            using (var unitOfWork = new UnitOfWork(nhConfiguration.SessionFactory))
            {
                var list = unitOfWork.Session.Query<PRODUCTO>().Take(100).ToList().ConvertListToDto<ProductDTO>();
                dtos = list.ToList();
            }
            
            Assert.Greater(dtos.Count(), 0);
        }
    }
}
