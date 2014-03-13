using System.Collections.Generic;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Dates;
using GenericMapper.Classes;
using GenericMapper.Interfaces;

namespace GenericMapper.Test
{
    public abstract class FixturePublic
    {
        protected const int CountList = 19;

        public virtual IDto GetInstanceDtoPublic()
        {
            return new DTO();
        }

        public virtual Entity GetEntityWithValuesEmpty()
        {
            return new Entity
                {
                    FirstProperty = string.Empty,
                    Second = string.Empty,
                    Third = new SubEntity
                        {
                            First = string.Empty,
                            Second = -1,
                            Third = string.Empty,
                            Fourth = new SubEntity2
                                {
                                    First = null
                                }
                        }
                };
        }

        public virtual Entity GetEntityWithValuesNull()
        {
            return new Entity
            {
                FirstProperty = null,
                Second = null,
                Third = new SubEntity
                {
                    First = null,
                    Second = -1,
                    Third = null,
                    Fourth = new SubEntity2
                    {
                        First = null
                    }
                }
            };
        }

        public virtual Entity GetEntityWithValues()
        {
            return Builder<Entity>.CreateNew()
               .With(x => x.Third = new SubEntity
               {
                   First = "Test 1",
                   Second = 2,
                   Third = "Test 2",
                   Fourth = new SubEntity2
                   {
                       First = 2
                   }
               })
               .Build();
        }

        public virtual SubEntity GetSubEntityWithValues()
        {
            return Builder<SubEntity>.CreateNew()
                .With(x => x.Second = 2)
                .With(x => x.Fourth = new SubEntity2
                    {
                        First = 2
                    }).Build();
        }

        public virtual IEnumerable<SubEntity> GetListSubEntityWithValues()
        {
            return Builder<SubEntity>.CreateListOfSize(CountList)
                .All()
                .With(x => x.Second = 2)
                .With(x => x.Fourth = new SubEntity2
                    {
                        First = 2
                    })
                .Build();
        }

        public virtual IEnumerable<Entity> GetListOfEntityWithValues()
        {
            return Builder<Entity>.CreateListOfSize(CountList)
               .All()
               .With(x => x.Third = new SubEntity
               {
                   First = "Test 1",
                   Second = 2,
                   Third = "Test 2",
                   Fourth = new SubEntity2
                   {
                       First = 2
                   }
               })
               .With(x=>x.Second= "Test 2")
               .Build();
        }

        public class DTO : PublicDTO, IDto { }

        public class PublicDTO
        {
            public string FirstProperty { get; set; }

            [Map("Second")]
            public string SecondProperty { get; set; }

            [Map("Third.Second")]
            public int ThirdWithSecond { get; set; }

            [Map("Third.Fourth.First")]
            public int? ThirdWithFourthWithSecond { get; set; }
        }

        public class Public2DTO : IDto
        {
            public string FirstProperty { get; set; }
            public string Second { get; set; }
            public SubEntity Third { get; set; }
        }

        public class Public3DTO : IDto
        {
            [Map("Second")]
            public string SecondProperty { get; set; }

            [Map("Third", true)]
            public DtoSubEntity SubEntityDto { get; set; }

            public SubEntity Third { get; set; }
        }

        public class Public5DTO : IDto
        {
            [Map("Second")]
            public string SecondProperty { get; set; }

            [Map("Third", true)]
            public DtoSubEntity SubEntityDto { get; set; }

            public SubEntity Third { get; set; }

            [Map("SubEntities", true)]
            public List<DtoSubEntity> SubEntities { get; set; }
        }

        public class DtoSubEntity : IDto
        {
            [Map("First")]
            public string One { get; set; }

            [Map("Second")]
            public int Two { get; set; }
        }

        public class Public4DTO : IDto
        {
            [Map("Second")]
            public EnumDto Two { get; set; }

            [Map("Second")]
            public int Second2 { get; set; }
        }

        public enum EnumDto
        {
            One = 1,
            Two = 2,
            Three = 3
        }

        #region Entity

        public class Entity
        {
            public string FirstProperty { get; set; }
            public string Second { get; set; }
            public SubEntity Third { get; set; }
            public List<SubEntity> SubEntities { get; set; }            
        }

        public class SubEntity
        {
            public string First { get; set; }
            public int Second { get; set; }
            public string Third { get; set; }
            public SubEntity2 Fourth { get; set; }
        }

        public class SubEntity2
        {
            public int? First { get; set; }
        }        
        #endregion
    }
}
