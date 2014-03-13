using System;
using System.Linq;
using System.Linq.Expressions;
using GenericMapper.Test.Extensions;
using NUnit.Framework;

namespace GenericMapper.Test.Tools
{
    [TestFixture]
    public class TestConvertExpression
    {
        readonly GenericMapper.Tools.Expressions _tool = new GenericMapper.Tools.Expressions();

        [Test]
        public void Validate_Expression_With_Customer_Property()
        {

            Expression<Func<FixturePublic.PublicDTO, bool>> expression = x => x.ThirdWithFourthWithSecond == 2;

            var data = new FixtureTools().GetListOfEntityWithValues();
            
            var expressionDto = _tool.ConvertToExpressionEntity<FixturePublic.PublicDTO, FixturePublic.Entity>(expression);
            var result = data.Where(expressionDto.Compile());

            Assert.Greater(result.Count(), 0);
        }

        [Test]
        public void Validate_Expression_With_Single_Property()
        {

            Expression<Func<FixturePublic.Public5DTO, bool>> expression = x => x.SecondProperty == "Test 2";

            var data = new FixtureTools().GetListOfEntityWithValues();
            var expressionEntity = _tool.ConvertToExpressionEntity<FixturePublic.Public5DTO, FixturePublic.Entity>(expression);
            var result = data.Where(expressionEntity.Compile());

            Assert.Greater(result.Count(), 0);
        }

        [Test]
        public void Validate_Expression_With_List_Of_DTO_Property()
        {
            Expression<Func<FixturePublic.Public5DTO, bool>> expression = x => x.SecondProperty == "" && x.SubEntities.Any(y => y.One == "FirstProperty1");

            Assert.Throws<Exception>(() => _tool.ConvertToExpressionEntity<FixturePublic.Public5DTO, FixturePublic.Entity>(expression));
        }
    }
}
