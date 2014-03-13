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
        #region Entity Methods

        public void ConvertToEntity<TDto, TEntity>(TDto dto, ref TEntity entity)
            where TDto : class
            where TEntity : class, new()
        {
            if (dto == null)
            {
                entity = new TEntity();
                return;
            }

            foreach (var p in dto.GetType().GetProperties())
            {
                LogicDtoToEntity(dto, entity, p);
            }
        }

        internal void ConvertToEntity<TDto, TEntity>(TDto dto, TEntity entity)
            where TDto : class
            where TEntity : class, new()
        {
            if (dto == null)
            {
                entity = new TEntity();
                return;
            }

            foreach (var p in dto.GetType().GetProperties())
            {
                LogicDtoToEntity(dto, entity, p);
            }
        }

        public TEntity ConvertToEntity<TDto, TEntity>(TDto dto)
            where TDto : class
            where TEntity : class, new()
        {
            if (dto == null)
                return new TEntity();

            var entity = new TEntity();
            foreach (var p in dto.GetType().GetProperties())
            {
                LogicDtoToEntity(dto, entity, p);
            }

            return entity;
        }

        public IEnumerable<TEntity> ConvertListToEntity<TDto, TEntity>(IEnumerable<TDto> dtos)
            where TDto : class
            where TEntity : class, new()
        {
            if (dtos == null)
                return new List<TEntity>();

            return dtos == null
                    ? new List<TEntity>()
                    : dtos.Select(ConvertToEntity<TDto, TEntity>);
        }

        public void ConvertListToEntity<TDto, TEntity>(IEnumerable<TDto> dtos, ref IEnumerable<TEntity> entities)
            where TDto : class
            where TEntity : class, new()
        {
            if (dtos == null && entities == null)
            {
                entities = new List<TEntity>();
                return;
            }

            entities = dtos == null
                        ? new List<TEntity>()
                        : dtos.Select(ConvertToEntity<TDto, TEntity>);
        }

        internal void ConvertListToEntity<TDto, TEntity>(IEnumerable<TDto> dtos, IEnumerable<TEntity> entities)
            where TDto : class
            where TEntity : class, new()
        {
            if (dtos == null && entities == null)
            {
                entities = new List<TEntity>();
                return;
            }

            entities = dtos == null
                        ? new List<TEntity>()
                        : dtos.Select(ConvertToEntity<TDto, TEntity>);
        }

        public void ConvertListToEntity<TDto, TEntity>(IEnumerable<TDto> dtos, ref List<TEntity> entities)
            where TDto : class
            where TEntity : class, new()
        {
            if (dtos == null && entities == null)
            {
                entities = new List<TEntity>();
                return;
            }

            entities = dtos == null
                        ? new List<TEntity>()
                        : dtos.Select(ConvertToEntity<TDto, TEntity>).ToList();
        }

        #endregion

        #region Private Methods

        private void LogicDtoToEntity<TDto, TEntity>(TDto dto, TEntity entity, PropertyInfo p)
            where TDto : class
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
                                    LogicDtoToEntity(dto, entity, propertyEntitySetValue);
                                }
                            }
                        }

                        //Instantiate the instance
                        var newEntity = Activator.CreateInstance(propertyEntity.PropertyType);
                        ConvertToEntity(p.GetValue(dto), newEntity);
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
                }
                else if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type type = propertyEntity.PropertyType.GetGenericArguments()[0];
                    Type listType = typeof(List<>).MakeGenericType(type);
                    var list = (IList)Activator.CreateInstance(listType);
                    foreach (var item in (IEnumerable)p.GetValue(dto))
                    {
                        var subEntity = Activator.CreateInstance((list.GetType().GetGenericArguments()[0]));
                        ConvertToEntity(item, subEntity);
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
                        if (propertyEntity.PropertyType.IsClass)
                            SetValueEntity(dto, entity, p, propertyEntity, namesProperties, 0);
                        else
                            propertyEntity.SetValue(entity, GetValue(dto, p, namesProperties, 0));

                }
                catch
                {
                    throw new Exception("Dto does not have correct value");
                }
            }
        }

        private static void SetValueEntity(object dto, object entity, PropertyInfo property, PropertyInfo propertyEntity, string[] namesProperties, int index)
        {
            while (property != null)
            {
                if (namesProperties.Length == (index + 1))
                {
                    if (entity != null)
                    {
                        propertyEntity.SetValue(entity, property.GetValue(dto)); //When the value to map is null
                        return;
                    }
                }

                if (!entity.GetType().GetProperty(namesProperties[index]).PropertyType.IsClass) continue;

                var nameNextProperty = namesProperties[index + 1];

                var newEntity = Activator.CreateInstance(entity.GetType().GetProperty(namesProperties[index]).PropertyType);
                propertyEntity.SetValue(entity, newEntity);
                index++;
                entity = newEntity;
                propertyEntity = propertyEntity.PropertyType.GetProperty(nameNextProperty);
            }
        }

        #endregion
    }
}
