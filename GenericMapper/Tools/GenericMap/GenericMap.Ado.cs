using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using GenericMapper.Classes;

namespace GenericMapper.Tools
{
    public partial class GenericMap
    {
        public List<TDTO> MapListFromDataSet<TDTO>(DataSet ds)
            where TDTO : class, new()
        {
            var list = new List<TDTO>();

            foreach (var row in ds.Tables[0].Rows)
            {
                var dto = new TDTO();
                foreach (var property in dto.GetType().GetProperties())
                {
                    LogicDataRowToDto(dto, (DataRow) row, property, null);
                }
                list.Add(dto);
            }
            return list;
        }

        public TDTO MapObjectFromDataSet<TDTO>(DataSet ds)
            where TDTO : class, new()
        {
            var dto = new TDTO();
            foreach (var row in ds.Tables[0].Rows)
            {
                foreach (var property in dto.GetType().GetProperties())
                {
                    LogicDataRowToDto(dto, (DataRow) row, property, null);
                }
                break;
            }

            return dto;
        }

        public List<TDTO> MapListFromDataTable<TDTO>(DataTable table)
            where TDTO : class, new()
        {
            var list = new List<TDTO>();

            foreach (var row in table.Rows)
            {
                var dto = new TDTO();
                foreach (var property in dto.GetType().GetProperties())
                {
                    LogicDataRowToDto(dto, (DataRow) row, property, null);
                }
                list.Add(dto);
            }
            return list;
        }

        public TDTO MapObjectFromDataTable<TDTO>(DataTable table)
            where TDTO : class, new()
        {
            var dto = new TDTO();
            foreach (var row in table.Rows)
            {
                foreach (var property in dto.GetType().GetProperties())
                {
                    LogicDataRowToDto(dto, (DataRow) row, property, null);
                }
                break;
            }
            return dto;
        }

        public List<TDTO> MapListFromDataReader<TDTO>(IDataReader reader)
            where TDTO : class, new()
        {
            var list = new List<TDTO>();

            while (reader.Read())
            {
                var dto = new TDTO();
                foreach (var property in dto.GetType().GetProperties())
                {
                    LogicDataReaderToDto(dto, reader, property, null);
                }
                list.Add(dto);
            }
            return list;
        }

        public TDTO MapObjectFromDataReader<TDTO>(IDataReader reader)
            where TDTO : class, new()
        {
            var dto = new TDTO();

            while (reader.Read())
            {
                foreach (var property in dto.GetType().GetProperties())
                {
                    LogicDataReaderToDto(dto, reader, property, null);
                }
                break;
            }
            return dto;
        }

        private void LogicDataRowToDto<TDto>(TDto dto, DataRow dr, PropertyInfo p, Type originalType)
            where TDto : class, new()
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
                namesProperties = attribute.NameProperty.Split(SeparateChar);
                lengthProperties = namesProperties.Length;
            }

            var nameFirstProperty = namesProperties[0];

            if (lengthProperties == 1)
            {
                if (p.PropertyType == originalType) return;

                //When fill property entity
                if (attribute != null && attribute.IsDto && !string.IsNullOrEmpty(attribute.NameRelation))
                {
                    // INITIALIZE EMPTY DTO
                    var intiailValue = Activator.CreateInstance(p.PropertyType);
                    p.SetValue(dto, intiailValue);

                    if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof (List<>))
                    {
                        var list = (IList) Activator.CreateInstance(p.PropertyType);
                        foreach (var row in dr.GetChildRows(attribute.NameRelation))
                        {
                            var subDto = Activator.CreateInstance((list.GetType().GetGenericArguments()[0]));
                            foreach (var pr in subDto.GetType().GetProperties())
                            {
                                LogicDataRowToDto(subDto, row, pr, dto.GetType());
                            }
                            list.Add(subDto);
                        }
                        p.SetValue(dto, list);
                    }
                    else
                    {
                        if (p.PropertyType.IsClass && attribute.IsDto)
                        {
                            foreach (var row in dr.GetChildRows(attribute.NameRelation))
                            {
                                var newDto = Activator.CreateInstance(p.PropertyType);
                                foreach (var pr in newDto.GetType().GetProperties())
                                {
                                    LogicDataRowToDto(newDto, row, pr, dto.GetType());
                                }
                                p.SetValue(dto, newDto);
                            }
                        }

                    }
                }
                else
                {
                    if (!p.PropertyType.IsClass &&
                        !(p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof (List<>)))
                    {
                        if (dr.Table.Columns.Contains(nameFirstProperty))
                            p.SetValue(dto, dr.Field<object>(nameFirstProperty));
                    }
                    else
                    {
                        if (p.PropertyType.BaseType == typeof (object))
                            if (dr.Table.Columns.Contains(nameFirstProperty))
                                p.SetValue(dto, dr.Field<object>(nameFirstProperty));
                    }
                }
            }
            else
            {
                try
                {
                    var nameRelations = attribute.NameRelation.Split(SeparateChar);
                    p.SetValue(dto, GetValueInDataRow(dr, p, namesProperties, nameRelations, 0));
                }
                catch
                {
                    throw new Exception("Entity does not have correct value");
                }
            }
        }

        private static object GetValueInDataRow(DataRow dr, PropertyInfo property, string[] namesProperties,
                                                string[] nameRelations, int index)
        {
            while (true)
            {
                if (namesProperties.Length == (index + 1))
                {
                    return dr == null ? null : dr.Field<object>(namesProperties[index]); //When the value to map is null
                }

                dr = dr.GetChildRows(nameRelations[index])[0];
                index++;
            }
        }

        private void LogicDataReaderToDto<TDto>(TDto dto, IDataReader dr, PropertyInfo p, Type originalType)
            where TDto : class, new()
        {
            var attribute = p.GetCustomAttribute<MapAttribute>();
            if (p.PropertyType == originalType) return;

            //When fill property entity
            p.SetValue(dto,
                       (attribute == null || string.IsNullOrEmpty(attribute.NameProperty))
                           ? dr[p.Name]
                           : dr[attribute.NameProperty]);
        }
    }
}
