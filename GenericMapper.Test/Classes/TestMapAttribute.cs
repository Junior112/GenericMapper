using System.Reflection;
using GenericMapper.Classes;
using NUnit.Framework;

namespace GenericMapper.Test.Classes
{
    [TestFixture]
    public class TestMapAttribute
    {
        [Test]
        public void ValidateMapAttribute()
        {
            var data = new FixtureClasses().GetInstanceDtoPublic();

            var property = data.GetType().GetProperty("SecondProperty");

            var attribute = property.GetCustomAttribute<MapAttribute>();

            Assert.IsNotNull(attribute);
            Assert.IsNotNullOrEmpty(attribute.NameProperty);
        }
    }
}
