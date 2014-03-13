using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GenericMapper.Classes;
using GenericMapper.Interfaces;

namespace GenericMapper.Helpers
{
    internal class GenericMap
    {
        public char SeparateChar = '.';

        #region Dtos Methods

        public void MapObject<TEntity, TDto>(TEntity entity, TDto dto)
            where TDto : class, IDto
            where TEntity : class
        {
            if(dto == null)
                throw new Exception("Object to map is null");

            if (entity == null)
                return;
            foreach (var p in dto.GetType().GetProperties())
            {
                LogicDtoToEntity<TEntity, TDto>(dto, entity, p, null);
            }
        }

        public void MapObject<TEntity, TDto>(TEntity entity, TDto dto, Type type)
            where TDto : class, IDto
            where TEntity : class
        {
            if (dto == null)
                throw new Exception("Object to map is null");

            if (entity == null)
                return;
            foreach (var p in dto.GetType().GetProperties())
            {
                LogicDtoToEntity<TEntity, TDto>(dto, entity, p, type);
            }
        }

        public IEnumerable<IDto> MapList<TEntity>(IEnumerable<TEntity> entities, IEnumerable<IDto> dtos)
            where TEntity : class
        {
            var utility = new Tools.GenericMap();
            return entities == null ? dtos
                                    : entities.Select(entity => entity != null 
                                                                ? (IDto)utility.GetMapObject(
                                                                                                entity, 
                                                                                                Activator.CreateInstance(dtos.GetType().GetGenericArguments()[0])
                                                                                             )
                                                                : (IDto)Activator.CreateInstance(
                                                                                                    dtos.GetType().GetGenericArguments()[0])
                                                                                                 );
        }

        #endregion

        #region Entity Methods

        public void ConvertToEntity<TEntity, TDto>(TDto dto, TEntity entity)
            where TDto : class, IDto
            where TEntity : class, new()
        {
            if (dto == null)
                throw new Exception("Object to map is null");

            foreach (var p in dto.GetType().GetProperties())
            {
               LogicEntityToDto(dto, entity, p);
            }
        }

        public TEntity ConvertToEntity<TEntity, TDto>(TDto dto)
            where TDto : class, IDto
            where TEntity : class, new()
        {
            if (dto == null)
                throw new Exception("Object to map is null");

            var entity = new TEntity();
            foreach (var p in dto.GetType().GetProperties())
            {
                LogicEntityToDto(dto, entity, p);
            }

            return entity;
        }

        public IEnumerable<TEntity> ConvertToEntity<TEntity, TDto>(IEnumerable<TDto> dtos)
            where TDto : class, IDto
            where TEntity : class, new()
        {
            return dtos == null
                    ? new List<TEntity>()
                    : dtos.Select(this.ConvertToEntity<TEntity, TDto>);
        }

        #endregion

        #region Private Methods

        private void LogicEntityToDto<TEntity, TDto>(TDto dto, TEntity entity, PropertyInfo p)
            where TDto : class, IDto
            where TEntity : class
        {
            var attribute = p.GetCustomAttribute<MapAttribute>();
            string[] namesProperties;
            int lengthProperties;
            if (attribute == null)
            {
                namesProperties = new string[1];
                namesProperties[0] = p.Name;
                lengthProperties = 1;
            }
            else
            {
                namesProperties = attribute.NameProperty.Split('.');
                lengthProperties = namesProperties.Length;
            }

            var nameFirstProperty = namesProperties[0];
            var propertyEntity = entity.GetType().GetProperties().FirstOrDefault(x => x.Name == nameFirstProperty);
            if (lengthProperties == 1)
            {
                if (propertyEntity == null) return;

                //When fill property entity
                if (attribute != null && attribute.IsDto)//TODO
                {
                    try
                    {
                        if (attribute.NamePropertyToInsert != string.Empty)
                        {
                            var namePropertyGetValue = p.GetValue(dto).GetType().GetProperties().FirstOrDefault(x => x.Name == attribute.NamePropertyToGet);
                            var propertyEntitySetValue = dto.GetType().GetProperties().FirstOrDefault(x => x.Name == attribute.NamePropertyToInsert);
                            if (propertyEntitySetValue != null)
                            {
                                if (namePropertyGetValue != null)
                                {
                                    propertyEntitySetValue.SetValue(dto, namePropertyGetValue.GetValue(p.GetValue(dto)));
                                    LogicEntityToDto(dto, entity, propertyEntitySetValue);
                                }
                            }
                        }

                        //Instantiate the instance
                        var newEntity = Activator.CreateInstance(propertyEntity.PropertyType);
                        ConvertToEntity((IDto)p.GetValue(dto), newEntity);
                        propertyEntity.SetValue(entity, newEntity);

                    }
                    catch
                    {
                        //TODO broke when a property is a interface and doesnt create a instance
                    }
                    return;
                }

                if (p.PropertyType == propertyEntity.PropertyType)
                {
                    propertyEntity.SetValue(entity, p.GetValue(dto));
                    return;
                }
                else if (p.PropertyType.BaseType == typeof(Enum))
                {
                    var value = 0;

                    value = (int)p.GetValue(dto);

                    if (Nullable.GetUnderlyingType(propertyEntity.PropertyType) != null)
                    {
                        if (propertyEntity.PropertyType == typeof(Int16?))
                            propertyEntity.SetValue(entity, (Int16?)value);
                        else
                            propertyEntity.SetValue(entity, (int?)value);
                    }
                    else
                        propertyEntity.SetValue(entity, value);

                    return;
                }else if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type type = propertyEntity.PropertyType.GetGenericArguments()[0];
                    Type listType = typeof (List<>).MakeGenericType(type); 
                    var list = (IList)Activator.CreateInstance(listType);
                    foreach (var item in (IEnumerable)p.GetValue(dto))
                    {
                        var subEntity = Activator.CreateInstance((list.GetType().GetGenericArguments()[0]));
                        ConvertToEntity((IDto)item, subEntity);
                        list.Add(subEntity);
                    }
                    propertyEntity.SetValue(entity, list);
                    return;
                }
            }
            else
            {
                try
                {
                    if (p == null || p.GetValue(dto) == null) return;
                    if (propertyEntity != null)
                        propertyEntity.SetValue(entity, GetValue(dto, p, namesProperties, 0));
                }
                catch
                {
                    throw new Exception("Dto does not have correct value");
                }
            }
        }

        private void LogicDtoToEntity<TEntity, TDto>(TDto dto, TEntity entity, PropertyInfo p, Type type)
            where TDto : class, IDto
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
                                    MapObject(propertyEntity.GetValue(entity), (IDto)newDto, dto.GetType());
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
                            MapObject(propertyEntity.GetValue(entity), (IDto)newDto, type);
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
                else if (p.PropertyType.BaseType == typeof(Enum))
                {
                    p.SetValue(dto, propertyEntity.GetValue(entity));
                    return;
                }
                else if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>)) //TODO:JUNIOR
                {
                    FillListOfDto<TEntity, TDto>(propertyEntity, entity, p, dto);
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
                    return property.GetValue(entity);
                }

                index++;
                entity = property.GetValue(entity);
                var nameNextProperty = namesProperties[index];
                property = property.PropertyType.GetProperties().FirstOrDefault(x => x.Name == nameNextProperty);
            }

            return null;
        }

        private void FillListOfDto<TEntity, TDto>(PropertyInfo propertyEntity, TEntity entity, PropertyInfo propertyDto, TDto dto)
            where TDto : class, IDto
            where TEntity : class
        {
            if (propertyEntity.GetValue(entity) == null) return;
            var list = (IList)Activator.CreateInstance(propertyDto.PropertyType);
            foreach (var item in (IEnumerable)propertyEntity.GetValue(entity))
            {
                var subDto = Activator.CreateInstance((list.GetType().GetGenericArguments()[0]));
                MapObject(item, (IDto)subDto);
                list.Add(subDto);
            }
            propertyDto.SetValue(dto, list);
        }

        #endregion
    }
}
