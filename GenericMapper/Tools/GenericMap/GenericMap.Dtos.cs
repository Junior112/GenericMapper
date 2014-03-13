using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GenericMapper.Classes;

namespace GenericMapper.Tools
{
    public partial class GenericMap : Interfaces.IGenericMap
    {
        #region Dtos Methods

        internal TDto GetMapObject<TEntity, TDto>(TEntity entity, TDto dto)
            where TEntity : class
            where TDto : class
        {
            if (dto == null)
                throw new Exception("Object to map is null");

            if (entity == null)
                return null;
            foreach (var p in dto.GetType().GetProperties())
            {
                LogicEntityToDto(dto, entity, p, null);
            }

            return dto;
        }

        public void MapObject<TEntity, TDto>(TEntity entity, TDto dto)
            where TEntity : class
            where TDto : class
        {
            if(dto == null)
                throw new Exception("Object to map is null");

            if (entity == null)
                return;
            foreach (var p in dto.GetType().GetProperties())
            {
                LogicEntityToDto<TEntity, TDto>(dto, entity, p, null);
            }
        }

        internal void MapObject<TEntity, TDto>(TEntity entity, TDto dto, Type type)
            where TEntity : class
            where TDto : class
        {
            if (dto == null)
                throw new Exception("Object to map is null");

            if (entity == null)
                return;

            foreach (var p in dto.GetType().GetProperties())
            {
                LogicEntityToDto<TEntity, TDto>(dto, entity, p, type);
            }
        }

        public TDto MapObject<TEntity, TDto>(TEntity entity)
            where TEntity : class
            where TDto : class, new()
        {
            if (entity == null)
                return new TDto();
            var dto = new TDto();
            MapObject(entity, dto);
            return dto;
        }

        public IEnumerable<TDto> MapList<TEntity, TDto>(IEnumerable<TEntity> entities)
            where TEntity : class
            where TDto : class, new()
        {
            return entities == null ? new List<TDto>()
                                    : entities.Select(entity => entity != null  ? MapObject<TEntity, TDto>(entity)
                                                                                : new TDto());
        }

        public void MapList<TEntity, TDto>(IEnumerable<TEntity> entities, ref IEnumerable<TDto> dtos)
            where TEntity : class
            where TDto : class, new()
        {
            dtos = entities == null 
                            ? new List<TDto>()
                            : entities.Select(entity => entity != null 
                                                                ? MapObject<TEntity, TDto>(entity)
                                                                : new TDto());
        }

        public void MapList<TEntity, TDto>(IEnumerable<TEntity> entities, ref List<TDto> dtos)
            where TEntity : class
            where TDto : class, new()
        {
            dtos = entities == null
                            ? new List<TDto>()
                            : entities.Select(entity => entity != null
                                                                ? MapObject<TEntity, TDto>(entity)
                                                                : new TDto()).ToList();
        }

        #endregion

        #region Privates methods

        private void LogicEntityToDto<TEntity, TDto>(TDto dto, TEntity entity, PropertyInfo p, Type type)
            where TDto : class
            where TEntity : class
        {
            var attribute = p.GetCustomAttribute<MapAttribute>();
            string[] namesProperties;
            int lengthProperties;

            //Esta parte es para validar cuando la propiedad tiene el map attribute 
            //y si no lo tiene se valida que el nombre y tipo de la propiedad coincida 
            //con el de la entidad y asi mapear
            if (attribute == null)
            {
                //No tiene el map attribute
                namesProperties = new string[1];
                namesProperties[0] = p.Name;
                lengthProperties = 1;
            }
            else
            {
                //Si tiene map attribute
                namesProperties = attribute.NameProperty.Split(SeparateChar);
                lengthProperties = namesProperties.Length;
            }

            var nameFirstProperty = namesProperties[0];
            var propertyEntity = entity.GetType().GetProperties().FirstOrDefault(x => x.Name == nameFirstProperty);
            if (lengthProperties == 1)
            {
                if (propertyEntity == null) return;

                //When fill property dto
                if (attribute != null && attribute.IsDto)
                {
                    //First time to map
                    if (type == null)
                    {
                        //Is a list type
                        if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            FillListOfDto(propertyEntity, entity, p, dto);
                            return;
                        }

                        //Instantiate the instance
                        var obj = propertyEntity.GetValue(entity);
                        if (obj == null)
                        {
                            // INITIALICE EMPTY DTO
                            var newDto = Activator.CreateInstance(p.PropertyType);
                            p.SetValue(dto, newDto);
                        }

                        if (obj != null)
                        {
                            if (obj.GetType().IsClass && attribute.IsDto) //Se mapea una subentidad solo si es de tipo dto
                            {
                                var newDto = Activator.CreateInstance(p.PropertyType);
                                try
                                {
                                    MapObject(propertyEntity.GetValue(entity), newDto, dto.GetType());
                                }
                                catch (Exception)
                                {
                                    throw;
                                    //TODO: check why but we can continuos
                                }
                                p.SetValue(dto, newDto);
                            }
                        }
                        return;
                    }
                    else
                    {
                        //Instantiate the instance
                        if (type == dto.GetType())
                            return;
                        var obj = propertyEntity.GetValue(entity);
                        if (obj.GetType().IsClass)
                        {
                            var newDto = Activator.CreateInstance(p.PropertyType);
                            MapObject(propertyEntity.GetValue(entity), newDto, type);
                            p.SetValue(dto, newDto);
                        }
                        return;
                    }
                }

                if (p.PropertyType == propertyEntity.PropertyType)
                {
                    p.SetValue(dto, propertyEntity.GetValue(entity));
                    return;
                }
                if (p.PropertyType.BaseType == typeof(Enum))
                {
                    p.SetValue(dto, propertyEntity.GetValue(entity));
                    return;
                }
                if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>)) //TODO:JUNIOR
                {
                    FillListOfDto(propertyEntity, entity, p, dto);
                }
            }
            else
            {
                try
                {
                    if (propertyEntity != null && propertyEntity.GetValue(entity) != null)
                        p.SetValue(dto, GetValue(entity, propertyEntity, namesProperties, 0));
                }
                catch
                {
                    throw new Exception("Entity does not have correct value");
                }
            }
        }


        private static object GetValue(object entity, PropertyInfo property, string[] namesProperties, int index)
        {
            while (property != null)
            {
                if (namesProperties.Length == (index + 1))
                {
                    return entity == null ? null : property.GetValue(entity); //When the value to map is null
                }

                index++;
                entity = property.GetValue(entity);
                var nameNextProperty = namesProperties[index];
                property = property.PropertyType.GetProperties().FirstOrDefault(x => x.Name == nameNextProperty);
            }

            return null;
        }

        private void FillListOfDto<TEntity, TDto>(PropertyInfo propertyEntity, TEntity entity, PropertyInfo propertyDto, TDto dto)
            where TEntity : class
            where TDto : class
        {
            if (propertyEntity.GetValue(entity) == null) return;
            var list = (IList)Activator.CreateInstance(propertyDto.PropertyType);
            foreach (var item in (IEnumerable)propertyEntity.GetValue(entity))
            {
                var subDto = Activator.CreateInstance((list.GetType().GetGenericArguments()[0]));
                MapObject(item, subDto);
                list.Add(subDto);
            }
            propertyDto.SetValue(dto, list);
        }

        #endregion
    }
}
