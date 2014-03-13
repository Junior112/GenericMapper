using FluentAssertions;
using GenericMapper.Extensions;
using NUnit.Framework;
using System.Linq;

namespace GenericMapper.Test.Extensions
{
    [TestFixture]
    public class TestMapping
    {
        #region Dto to Entity

        /// <summary>
        /// This test is for validate the map when the name of the properties (Entity and Dto) are equal
        /// </summary>
        [Test]
        public void Validate_Map_List_Entity_To_Dto_Only_The_First_Property()
        {
            var listOfEntity = new FixtureExtensions().GetListOfEntityWithValuesInFirstAndSecondProperty();
            var expectValues = listOfEntity.Select(x => x.GetType().GetProperty("FirstProperty").GetValue(x));

            var listOfDtos = listOfEntity.ConvertListToDto<FixtureExtensions.DTO>();

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

            var listOfDtos = listOfEntity.ConvertListToDto<FixtureExtensions.DTO>();

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
            var listOfDtos = listOfEntity.ConvertListToDto<FixtureExtensions.DTO>();

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
                                                          .GetValue( x.GetType()
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
            var listOfDtos = listOfEntity.ConvertListToDto<FixtureExtensions.DTO>();
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
            var actual = data.ConvertListToDto<FixturePublic.Public2DTO>();

            var values = actual.Select(y=>y.GetType().GetProperties().Select(x => x.GetValue(actual)));
            values.Should().NotContainNulls();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Validate_Dto_When_The_Properties_Are_Equals()
        {
            var data = new FixtureExtensions().GetEntityWithValues();
            var actual = data.ConvertToDto<FixturePublic.Public2DTO>();

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
            var actual = data.ConvertToDto<FixturePublic.Public3DTO>();

            var values = actual.GetType().GetProperties().Select(x => x.GetValue(actual));
            values.Should().NotContainNulls();
        }

        [Test]
        public void Validate_List_Of_Dto_With_Values_Enums()
        {
            var data = new FixtureExtensions().GetListSubEntityWithValues();
            var actual = data.ConvertListToDto<FixturePublic.Public4DTO>();

            var values = actual.Select(y => y.GetType().GetProperties().Select(x => x.GetValue(actual)));
            values.Should().NotContainNulls();
        }

        [Test]
        public void Validate_Dto_With_Values_Enums()
        {
            var data = new FixtureExtensions().GetSubEntityWithValues();
            var actual = data.ConvertToDto<FixturePublic.Public4DTO>();

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

            var listOfDtos = listOfEntity.ConvertListToDto<FixtureExtensions.DTO>();
            var listOfNewEntities = listOfDtos.ConvertListToEntity<FixtureExtensions.Entity>();

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

            var listOfDtos = listOfEntity.ConvertListToDto<FixtureExtensions.DTO>();
            var listOfNewEntities = listOfDtos.ConvertListToEntity<FixtureExtensions.Entity>();

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
            var listOfDtos = listOfEntity.ConvertListToDto<FixtureExtensions.DTO>();
            var listOfNewEntities = listOfDtos.ConvertListToEntity<FixtureExtensions.Entity>();

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
            var listOfDtos = listOfEntity.ConvertListToDto<FixtureExtensions.DTO>();
            var listOfNewEntities = listOfDtos.ConvertListToEntity<FixtureExtensions.Entity>();

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
            var dto = data.ConvertListToDto<FixturePublic.Public2DTO>();
            var actual = dto.ConvertListToEntity<FixturePublic.Entity>();

            var values = actual.Select(y => y.GetType().GetProperties().Select(x => x.GetValue(actual)));
            values.Should().NotContainNulls();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Validate_Entity_When_The_Properties_Are_Equals()
        {
            var data = new FixtureExtensions().GetEntityWithValues();
            var dto = data.ConvertToDto<FixturePublic.Public2DTO>();
            var actual = dto.ConvertToEntity<FixturePublic.Entity>();

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
            var dto = data.ConvertToDto<FixturePublic.Public3DTO>();
            var actual = dto.ConvertToEntity<FixturePublic.Entity>();

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
            var dto = data.ConvertListToDto<FixturePublic.Public4DTO>();
            var actual = dto.ConvertListToEntity<FixturePublic.SubEntity>();

            var values = actual.Select(y => y.GetType().GetProperties().Select(x => x.GetValue(actual)));
            values.Should().NotContainNulls();
        }

        [Test]
        public void Validate_Entity_With_Values_Enums()
        {
            var data = new FixtureExtensions().GetSubEntityWithValues();
            var dto = data.ConvertToDto<FixturePublic.Public4DTO>();
            var actual = dto.ConvertToEntity<FixturePublic.SubEntity>();

            Assert.IsTrue(actual.Second == data.Second);
        }

        #endregion

        [Test]
        public void Validate_Entity_When_Dto_Is_Null()
        {
            FixturePublic.Public4DTO dto = null;
            var actual = dto.ConvertToEntity<FixturePublic.SubEntity>();

            Assert.IsNotNull(actual);
        }     
        
        [Test]
        public void Validate_Dto_When_Entity_Is_Null()
        {
            FixturePublic.Entity entity = null;
            var actual = entity.ConvertToDto<FixturePublic.DTO>();

            Assert.IsNotNull(actual);
        }
    }
}