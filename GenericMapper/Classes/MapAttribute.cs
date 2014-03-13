using System;

namespace GenericMapper.Classes
{
    public class MapAttribute : Attribute
    {
        /// <summary>
        /// Propiedad de mapeo directo
        /// </summary>
        public string NameProperty { get; set; }

        /// <summary>
        /// Propiedad donde se insertara el valor de la sub entidad
        /// </summary>
        public string NamePropertyToInsert { get; set; }
        
        /// <summary>
        /// Propiedad de la sub entidad de donde se obtendra el valor 
        /// para posteriormente insertar en la entidad de origen
        /// </summary>
        public string NamePropertyToGet { get; set; }
        
        /// <summary>
        /// Especifica si esa propiedad es un DTO y asi tambien mapear esa propiedad y regresar un dto y no una entidad
        /// Si el valor es false, la propiedad es ignorada y no se mapea (es para clases)
        /// </summary>
        public bool IsDto { get; set; }

        public string NamePropertyId { get; set; }

        /// <summary>
        /// Is for when we use Data Set mappings
        /// </summary>
        public string NameRelation { get; set; }
        /// <summary>
        /// Mapping
        /// </summary>
        /// <param name="nameProperty"></param>
        /// <param name="isDto"></param>
        /// <param name="namePropertyToInsert">Propiedad a donde se inserta el valor a nivel incial ( id con el que relaciona la entidad con la subentidad ) 
        /// y tambien sirve para hacer el inner join (apuntando a DTO no a Entidad)</param>
        /// <param name="namePropertyToGet">Propiedad donde se traera el valor e insertara en el Dto a nivel inicial (apuntando a DTO no a Entidad)</param>
        /// <param name="namePropertyId">Id de la Dto para hacer el inner join (apuntando a DTO no a Entidad)</param>
        /// <param name="namePropertyToSearch">Propiedad donde se hara la busqueda global (apuntando a DTO no a Entidad)</param>
        /// <param name="isOnlyBySearch"> Nos ayuda a saber a no mapear la propiedad ya que solo es para el search (ayuda al performances) </param>
        public MapAttribute(string nameProperty, bool isDto = false, string namePropertyToInsert = "", string namePropertyToGet = "", string namePropertyId = "", string nameRelation = "")
        {
            NameProperty = nameProperty;
            NamePropertyToGet = namePropertyToGet;
            NamePropertyToInsert = namePropertyToInsert;
            NamePropertyId = namePropertyId;
            IsDto = isDto;
            NameRelation = nameRelation;
        }
    }
}