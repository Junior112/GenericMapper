using System.Collections.Generic;
using System.Data;
using ADOTest.DTOs;
using ADOTest.Entities;
using GenericMapper.Tools;
using NUnit.Framework;

namespace ADOTest.Test
{
    [TestFixture]
    public class ProductTest
    {
        [Test]
        public void NewProduct()
        {
            var service = new DataServiceSpanish();
            service.Conectar();
            var ds = service.ExecuteDataSet("select * from producto where id = 1; Select * from linea where Idproducto = 1;");
            ds.Relations.Add("ProductoLinea", ds.Tables[0].Columns["Id"], ds.Tables[1].Columns["IdProducto"]);

            var mapper = new GenericMap();
            var list = mapper.MapListFromDataSet<PRODUCTO>(ds);
            service.Desconectar();
            Assert.Greater(list.Count, 0);
        }

        [Test]
        public void NewProductWithStored()
        {
            var service = new DataServiceSpanish();
            service.Conectar();
            var ds = service.ExecuteDataSet("GetProduct", CommandType.StoredProcedure, new Dictionary<string, object> { { "@IdProduct", 1 } });
            ds.Relations.Add("ProductoLinea", ds.Tables[0].Columns["Id"], ds.Tables[1].Columns["IdProducto"]);

            var mapper = new GenericMap();
            var list = mapper.MapListFromDataSet<PRODUCTO>(ds);
            service.Desconectar();
            Assert.Greater(list.Count, 0);
        }

        [Test]
        public void NewLinea()
        {
            var service = new DataServiceSpanish();
            service.Conectar();
            var ds = service.ExecuteDataSet("select * from linea where id = 1; Select * from producto where id in (select idproducto from linea where id = 1);");
            ds.Relations.Add("LineaProducto", ds.Tables[0].Columns["IdProducto"], ds.Tables[1].Columns["Id"]);

            var mapper = new GenericMap();
            var list = mapper.MapListFromDataSet<LINEA>(ds);
            service.Desconectar();
            Assert.Greater(list.Count, 0);

        }


        [Test]
        public void NewLineaDTO()
        {
            var service = new DataServiceSpanish();
            service.Conectar();
            var ds = service.ExecuteDataSet("select * from linea where id = 1; Select * from producto where id in (select idproducto from linea where id = 1);");
            ds.Relations.Add("LineaProducto", ds.Tables[0].Columns["IdProducto"], ds.Tables[1].Columns["Id"]);

            var mapper = new GenericMap();
            var list = mapper.MapListFromDataSet<Line2DTO>(ds);
            service.Desconectar();
            Assert.Greater(list.Count, 0);

        }


        [Test]
        public void NewLinea2DTO()
        {
            var service = new DataServiceSpanish();
            service.Conectar();
            var ds = service.ExecuteDataSet("select * from producto where id = 1; Select * from linea where Idproducto = 1; Select * from ALBARAN_LINEA where idlinea in (Select ID from linea where Idproducto = 1);");
            ds.Relations.Add("ProductoLinea", ds.Tables[0].Columns["Id"], ds.Tables[1].Columns["IdProducto"]);
            ds.Relations.Add("LineaLineaAlbaran", ds.Tables[1].Columns["Id"], ds.Tables[2].Columns["IdLinea"]);

            var mapper = new GenericMap();
            var list = mapper.MapListFromDataSet<ProductDTO>(ds);
            service.Desconectar();
            Assert.Greater(list.Count, 0);

        }
    }
}
