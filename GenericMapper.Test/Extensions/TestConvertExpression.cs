using System;
using System.Linq;
using System.Linq.Expressions;
using GenericMapper.Extensions;
using NUnit.Framework;

namespace GenericMapper.Test.Extensions
{
    public class TestConvertExpression
    {

        [Test]
        public void Validate_Expression_With_Customer_Property()
        {

            Expression<Func<FixturePublic.PublicDTO, bool>> expression = x => x.ThirdWithFourthWithSecond == 2;

            var data = new FixtureExtensions().GetListOfEntityWithValues();
            var expressionDto = expression.ConvertToExpressionEntity<FixturePublic.PublicDTO, FixturePublic.Entity>();
            var result = data.Where(expressionDto.Compile());

            Assert.Greater(result.Count(), 0);
        }

        [Test]
        public void Validate_Expression_With_Single_Property()
        {

            Expression<Func<FixturePublic.Public5DTO, bool>> expression = x => x.SecondProperty == "Test 2";

            var data = new FixtureExtensions().GetListOfEntityWithValues();
            var expressionEntity = expression.ConvertToExpressionEntity<FixturePublic.Public5DTO, FixturePublic.Entity>();
            var result = data.Where(expressionEntity.Compile());

            Assert.Greater(result.Count(), 0);
        }

        [Test]
        public void Validate_Expression_With_List_Of_DTO_Property()
        {
            Expression<Func<FixturePublic.Public5DTO, bool>> expression = x => x.SecondProperty == "" && x.SubEntities.Any(y => y.One == "FirstProperty1");

            Assert.Throws<Exception>(() => expression.ConvertToExpressionEntity<FixturePublic.Public5DTO, FixturePublic.Entity>());
        }
    }
}