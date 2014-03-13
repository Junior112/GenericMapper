using System.Collections.Generic;
using FizzWare.NBuilder;

namespace GenericMapper.Test.Extensions
{
    public class FixtureExtensions : FixturePublic
    {
        public virtual IEnumerable<Entity> GetListOfEntityWithValuesInFirstAndSecondProperty()
        {
            return Builder<Entity>.CreateListOfSize(CountList).All().Build();
        }

        public virtual IEnumerable<Entity> GetListEntityWithValuesInThirdProperty()
        {
            return Builder<Entity>.CreateListOfSize(CountList)
                .All()
                .With(x => x.Third = new SubEntity
                {
                    First = "Sub Entity Test 1",
                    Second = 2,
                    Third = "Sub Entity Test 1"
                })
                .Build();
        }

        public virtual IEnumerable<Entity> GetListEntityWithValuesInFourthProperty()
        {
            return Builder<Entity>.CreateListOfSize(CountList)
                .All()
                .With(x => x.Third = new SubEntity
                { 
                    Fourth = new SubEntity2
                    {
                        First = 2
                    }
                })
                .Build();
        }
    }

    public class DToExtention : FixturePublic.DTO
    {
    }
}
