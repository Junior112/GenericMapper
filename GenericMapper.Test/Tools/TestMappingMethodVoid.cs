using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GenericMapper.Interfaces;
using GenericMapper.Test.Extensions;
using GenericMapper.Tools;
using NUnit.Framework;

namespace GenericMapper.Test.Tools
{
    [TestFixture]
    public class TestMappingMethodVoid
    {
        private readonly IGenericMap _tool = new GenericMap();

        #region Dto to Entity

        /// <summary>
        /// This test is for validate the map when the name of the properties (Entity and Dto) are equal
        /// </summary>
        [Test]
        public void Validate_Map_List_Entity_To_Dto_Only_The_First_Property()
        {
            var listOfEntity = new FixtureExtensions().GetListOfEntityWithValuesInFirstAndSecondProperty();
            var expectValues = listOfEntity.Select(x => x.GetType().GetProperty("FirstProperty").GetValue(x));

            var listOfDtos = new List<FixturePublic.DTO>();
            _tool.MapList(listOfEntity, ref listOfDtos);

            var values = listOfDtos.Select(x => x.GetType().GetProperty("FirstProperty").GetValue(x));

            values.ShouldAllBeEquivalentTo(expectValues);
        }

        /// <summary>
        /// This test is for validate a simple map
        /// </summary>
        [Test]
        public void Validate_Map_List_Entity_To_Dto_Only_The_Second_Property()
        {
            var listOfEntity = new FixtureExtensions().GetListOfEntityWithValuesInFirstAndSecondProperty();
            var expectValues = listOfEntity.Select(x => x.GetType().GetProperty("Second").GetValue(x));

            var listOfDtos = new List<FixturePublic.DTO>();
            _tool.MapList(listOfEntity, ref listOfDtos);

            var values = listOfDtos.Select(x => x.GetType().GetProperty("SecondProperty").GetValue(x));

            values.ShouldAllBeEquivalentTo(expectValues);
        }

        /// <summary>
        /// This test is for validate when the property is from sub entity
        /// </summary>
        [Test]
        public void Validate_Map_List_Entity_To_Dto_Only_The_Third_Property()
        {
            var listOfEntity = new FixtureExtensions().GetListEntityWithValuesInThirdProperty();

            //Get the values from the property "Second" from the property "Third" 
            var expectValues = listOfEntity.Select(x => x.GetType()
                                                            .GetProperty("Third")
                                                            .GetValue(x)
                                                            .GetType()
                                                            .GetProperty("Second")
                                                            .GetValue(x.GetType()
                                                                          .GetProperty("Third")
                                                                          .GetValue(x))
                );
            //Convert all de list of entities to list of dtos
            var listOfDtos = new List<FixturePublic.DTO>();
            _tool.MapList(listOfEntity, ref listOfDtos);

            var values = listOfDtos.Select(x => x.GetType().GetProperty("ThirdWithSecond").GetValue(x));

            values.ShouldAllBeEquivalentTo(expectValues);
        }

        /// <summary>
        /// This test is for validate when the property is from sub entity
        /// </summary>
        [Test]
        public void Validate_Map_List_Entity_To_Dto_Only_The_Fourth_Property()
        {
            var listOfEntity = new FixtureExtensions().GetListEntityWithValuesInFourthProperty();

            //Get the values from the property "First" from the property "Fourth" from the property "Third" 
            var expectValues = listOfEntity.Select(x => x.GetType()
                                                            .GetProperty("Third")
                                                            .GetValue(x)
                                                            .GetType()
                                                            .GetProperty("Fourth")
                                                            .GetValue(x.GetType()
                                                                          .GetProperty("Third")
                                                                          .GetValue(x)
                                                            )
                                                            .GetType()
                                                            .GetProperty("First")
                                                            .GetValue(x.GetType()
                                                                          .GetProperty("Third")
                                                                          .GetValue(x)
                                                                          .GetType()
                                                                          .GetProperty("Fourth")
                                                                          .GetValue(x.GetType()
                                                                                        .GetProperty("Third")
                                                                                        .GetValue(x))
                                                            )
                );

            //Convert all de list of entities to list of dtos
            var listOfDtos = new List<FixturePublic.DTO>();
            _tool.MapList(listOfEntity, ref listOfDtos);

            //Get all values from the la property "ThirdWithFourthWithSecond"
            var values = listOfDtos.Select(x => x.GetType().GetProperty("ThirdWithFourthWithSecond").GetValue(x));

            values.ShouldAllBeEquivalentTo(expectValues);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Validate_List_Dto_When_The_Properties_Are_Equals()
        {
            var data = new FixtureExtensions().GetListOfEntityWithValues();
            var actual = _tool.MapList<FixturePublic.Entity, FixturePublic.Public2DTO>(data);
            var dto = new FixturePublic.Public2DTO();
            _tool.MapObject(actual, dto);

            var values = actual.Select(y => y.GetType().GetProperties().Select(x => x.GetValue(actual)));
            values.Should().NotContainNulls();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Validate_Dto_When_The_Properties_Are_Equals()
        {
            var data = new FixtureExtensions().GetEntityWithValues();
            var actual = new FixturePublic.Public2DTO();
            _tool.MapObject(data, actual);

            var values = actual.GetType().GetProperties().Select(x => x.GetValue(actual));
            values.Should().NotContainNulls();
        }

        /// <summary>
        /// This test is for validate when a dto has a sub dto and a sub entity.
        /// </summary>
        [Test]
        public void Validate_Dto_When_Sub_Entity_Converted_Dto()
        {
            var data = new FixtureExtensions().GetEntityWithValues();
            var actual = new FixturePublic.Public3DTO();
            _tool.MapObject(data, actual);

            var values = actual.GetType().GetProperties().Select(x => x.GetValue(actual));
            values.Should().NotContainNulls();
        }

        [Test]
        public void Validate_List_Of_Dto_With_Values_Enums()
        {
            var data = new FixtureExtensions().GetListSubEntityWithValues();
            var actual = new List<FixturePublic.Public4DTO>();
            _tool.MapList(data, ref actual);

            var values = actual.Select(y => y.GetType().GetProperties().Select(x => x.GetValue(actual)));
            values.Should().NotContainNulls();
        }

        [Test]
        public void Validate_Dto_With_Values_Enums()
        {
            var data = new FixtureExtensions().GetSubEntityWithValues();
            var actual = new FixturePublic.Public4DTO();
            
            _tool.MapObject(data, actual);

            var values = actual.GetType().GetProperties().Select(x => x.GetValue(actual));
            values.Should().NotContainNulls();
        }

        #endregion

        #region Entity to Dto

        /// <summary>
        /// This test is for validate the map when the name of the properties (Entity and Dto) are equal
        /// </summary>
        [Test]
        public void Validate_Map_List_Dto_To_Entity_Only_The_First_Property()
        {
            var listOfEntity = new FixtureExtensions().GetListOfEntityWithValuesInFirstAndSecondProperty();
            var expectValues = listOfEntity.Select(x => x.GetType().GetProperty("FirstProperty").GetValue(x));

            var listOfDtos = _tool.MapList<FixturePublic.Entity, FixtureExtensions.DTO>(listOfEntity);

             var listOfNewEntities = new List<FixturePublic.Entity>();
            _tool.ConvertListToEntity(listOfDtos, ref listOfNewEntities);

            var values = listOfNewEntities.Select(x => x.GetType().GetProperty("FirstProperty").GetValue(x));

            values.ShouldAllBeEquivalentTo(expectValues);
        }

        /// <summary>
        /// This test is for validate a simple map
        /// </summary>
        [Test]
        public void Validate_Map_List_Dto_To_Entity_Only_The_Second_Property()
        {
            var listOfEntity = new FixtureExtensions().GetListOfEntityWithValuesInFirstAndSecondProperty();
            var expectValues = listOfEntity.Select(x => x.GetType().GetProperty("Second").GetValue(x));

            var listOfDtos = _tool.MapList<FixturePublic.Entity, FixtureExtensions.DTO>(listOfEntity);

            var listOfNewEntities = new List<FixturePublic.Entity>();
            _tool.ConvertListToEntity(listOfDtos, ref listOfNewEntities);


            var values = listOfNewEntities.Select(x => x.GetType().GetProperty("Second").GetValue(x));

            values.ShouldAllBeEquivalentTo(expectValues);
        }

        /// <summary>
        /// This test is for validate when the property is from sub entity
        /// </summary>
        [Test]
        public void Validate_Map_List_Dto_To_Entity_Only_The_Third_Property()
        {
            var listOfEntity = new FixtureExtensions().GetListEntityWithValuesInThirdProperty();

            //Get the values from the property "Second" from the property "Third" 
            var expectValues = listOfEntity.Select(x => x.GetType()
                                                            .GetProperty("Third")
                                                            .GetValue(x)
                                                            .GetType()
                                                            .GetProperty("Second")
                                                            .GetValue(x.GetType()
                                                                          .GetProperty("Third")
                                                                          .GetValue(x))
                );
            //Convert all de list of entities to list of dtos
            var listOfDtos = _tool.MapList<FixturePublic.Entity, FixtureExtensions.DTO>(listOfEntity);

            var listOfNewEntities = new List<FixturePublic.Entity>();
            _tool.ConvertListToEntity(listOfDtos, ref listOfNewEntities);


            var values = listOfNewEntities.Select(x => x.GetType()
                                                           .GetProperty("Third")
                                                           .GetValue(x)
                                                           .GetType()
                                                           .GetProperty("Second")
                                                           .GetValue(x.GetType()
                                                                         .GetProperty("Third")
                                                                         .GetValue(x))
                );

            values.ShouldAllBeEquivalentTo(expectValues);
        }

        /// <summary>
        /// This test is for validate when the property is from sub entity
        /// </summary>
        [Test]
        public void Validate_Map_List_Dto_To_Entity_Only_The_Fourth_Property()
        {
            var listOfEntity = new FixtureExtensions().GetListEntityWithValuesInFourthProperty();

            //Get the values from the property "First" from the property "Fourth" from the property "Third" 
            var expectValues = listOfEntity.Select(x => x.GetType()
                                                            .GetProperty("Third")
                                                            .GetValue(x)
                                                            .GetType()
                                                            .GetProperty("Fourth")
                                                            .GetValue(x.GetType()
                                                                          .GetProperty("Third")
                                                                          .GetValue(x)
                                                            )
                                                            .GetType()
                                                            .GetProperty("First")
                                                            .GetValue(x.GetType()
                                                                          .GetProperty("Third")
                                                                          .GetValue(x)
                                                                          .GetType()
                                                                          .GetProperty("Fourth")
                                                                          .GetValue(x.GetType()
                                                                                        .GetProperty("Third")
                                                                                        .GetValue(x))
                                                            )
                );
            //Convert all de list of entities to list of dtos
            var listOfDtos = _tool.MapList<FixturePublic.Entity, FixtureExtensions.DTO>(listOfEntity);

            var listOfNewEntities = new List<FixturePublic.Entity>();
            _tool.ConvertListToEntity(listOfDtos, ref listOfNewEntities);

            //Get all values from the la property "ThirdWithFourthWithSecond"
            var values = listOfNewEntities.Select(x => x.GetType()
                                                           .GetProperty("Third")
                                                           .GetValue(x)
                                                           .GetType()
                                                           .GetProperty("Fourth")
                                                           .GetValue(x.GetType()
                                                                         .GetProperty("Third")
                                                                         .GetValue(x)
                                                           )
                                                           .GetType()
                                                           .GetProperty("First")
                                                           .GetValue(x.GetType()
                                                                         .GetProperty("Third")
                                                                         .GetValue(x)
                                                                         .GetType()
                                                                         .GetProperty("Fourth")
                                                                         .GetValue(x.GetType()
                                                                                       .GetProperty("Third")
                                                                                       .GetValue(x))
                                                           )
                );

            values.ShouldAllBeEquivalentTo(expectValues);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Validate_List_Entity_When_The_Properties_Are_Equals()
        {
            var data = new FixtureExtensions().GetListOfEntityWithValues();
            var listOfDtos = _tool.MapList<FixturePublic.Entity, FixturePublic.Public2DTO>(data);

            var listOfNewEntities = new List<FixturePublic.Entity>();
            _tool.ConvertListToEntity(listOfDtos, ref listOfNewEntities);


            var values = listOfNewEntities.Select(y => y.GetType().GetProperties().Select(x => x.GetValue(listOfNewEntities)));
            values.Should().NotContainNulls();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Validate_Entity_When_The_Properties_Are_Equals()
        {
            var data = new FixtureExtensions().GetEntityWithValues();
            var dto = _tool.MapObject<FixturePublic.Entity, FixturePublic.Public2DTO>(data);
            var actual = new FixturePublic.Entity();
            
            _tool.ConvertToEntity(dto, ref actual);

            var values = actual
                .GetType()
                .GetProperty("Third")
                .GetValue(actual)
                .GetType()
                .GetProperties()
                .Select(x => x.GetValue(actual
                                            .GetType()
                                            .GetProperty("Third")
                                            .GetValue(actual)));
            values.Should().NotContainNulls();

            Assert.IsNotNullOrEmpty(actual.FirstProperty);
            Assert.IsNotNullOrEmpty(actual.Second);
        }

        /// <summary>
        /// This test is for validate when a dto has a sub dto and a sub entity.
        /// </summary>
        [Test]
        public void Validate_Entity_When_Sub_Entity_Converted_Dto()
        {
            var data = new FixtureExtensions().GetEntityWithValues();
            var dto = _tool.MapObject<FixturePublic.Entity, FixturePublic.Public3DTO>(data);
            var actual = new FixturePublic.Entity();

            _tool.ConvertToEntity(dto, ref actual);

            Assert.IsNotNull(actual.Third.Third);

            //Get all values from actual.Third.Third
            var values = actual
                .GetType()
                .GetProperty("Third")
                .GetValue(actual)
                .GetType()
                .GetProperties()
                .Select(x => x.GetValue(actual
                                            .GetType()
                                            .GetProperty("Third")
                                            .GetValue(actual)));
            values.Should().NotContainNulls();

        }

        [Test]
        public void Validate_List_Of_Entity_With_Values_Enums()
        {
            var data = new FixtureExtensions().GetListSubEntityWithValues();
            var dto = _tool.MapList<FixturePublic.SubEntity, FixturePublic.Public4DTO>(data);
            var actual = new List<FixturePublic.SubEntity>();

            _tool.ConvertToEntity(dto, ref actual);

            var values = actual.Select(y => y.GetType().GetProperties().Select(x => x.GetValue(actual)));
            values.Should().NotContainNulls();
        }

        [Test]
        public void Validate_Entity_With_Values_Enums()
        {
            var data = new FixtureExtensions().GetSubEntityWithValues();
            var dto = _tool.MapObject<FixturePublic.SubEntity, FixturePublic.Public4DTO>(data);
            var actual = new FixturePublic.SubEntity();

            _tool.ConvertToEntity(dto, ref actual);

            Assert.IsTrue(actual.Second == data.Second);
        }

        #endregion

        [Test]
        public void Validate_Entity_When_Dto_Is_Null()
        {
            FixturePublic.Public4DTO dto = null;
            var actual = new FixturePublic.SubEntity();

            _tool.ConvertToEntity(dto, ref actual);

            Assert.IsNotNull(actual);
        }

        [Test]
        public void Validate_Dto_When_Entity_Is_Null()
        {
            FixturePublic.Entity entity = null;
            var actual = new FixturePublic.DTO();

            _tool.MapObject(entity, actual);


            Assert.IsNotNull(actual);
        }
    }
}