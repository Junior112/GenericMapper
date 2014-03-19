using System;

namespace GenericMapper.Classes
{
    public class MapAttribute : Attribute
    {
        public string NameProperty { get; set; }

        public string NamePropertyToInsert { get; set; }
        
        public string NamePropertyToGet { get; set; }
        
        public bool IsDto { get; set; }

        /// <summary>
        /// Is for when we use Data Set mappings
        /// </summary>
        public string NameRelation { get; set; }
        /// <summary>
        /// Mapping
        /// </summary>
        /// <param name="nameProperty"></param>
        /// <param name="isDto"></param>
        /// <param name="namePropertyToInsert"></param>
        /// <param name="namePropertyToGet"></param>
        /// <param name="namePropertyToSearch"></param>
        public MapAttribute(string nameProperty, bool isDto = false, string namePropertyToInsert = "", string namePropertyToGet = "", string nameRelation = "")
        {
            NameProperty = nameProperty;
            NamePropertyToGet = namePropertyToGet;
            NamePropertyToInsert = namePropertyToInsert;
            IsDto = isDto;
            NameRelation = nameRelation;
        }
    }
}