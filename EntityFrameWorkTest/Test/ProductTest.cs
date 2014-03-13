using System.Collections.Generic;
using EntityFrameWorkTest.DTOs;
using GenericMapper.Interfaces;
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
            using (var db = new TestEntities())
            {
                var list = db.Set<PRODUCTO>().Take(100).ToList().ConvertListToDto<ProductDTO>();
                dtos = list.ToList();
            }
            Assert.Greater(dtos.Count(), 0);
        }
    }
}
