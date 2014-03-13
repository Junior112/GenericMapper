using System;
using System.Collections.Generic;
using System.Data;
using GenericMapper.Interfaces;
using GenericMapper.Tools;

namespace GenericMapper.Extensions
{
    public static class Mapping
    {
        public static TEntity ConvertToEntity<TEntity>(this IDto dto)
            where TEntity : class, new()
        {
            if (dto == null)
                return new TEntity();

            var entity = new TEntity();
            var utilMap = new GenericMap();

            utilMap.ConvertToEntity(dto, ref entity);

            return entity;
        }

        public static TDto ConvertToDto<TDto>(this object entity)
            where TDto : class, new()
        {
            if (entity == null)
                return new TDto();

            var dto = new TDto();
            var utilMap = new GenericMap();

            utilMap.MapObject(entity, dto);
            return dto;
        }

        public static IEnumerable<TEntity> ConvertListToEntity<TEntity>(this IEnumerable<IDto> dtos)
            where TEntity : class, new()
        {
            if (dtos == null)
                return new List<TEntity>();

            var utilMap = new GenericMap();

            return utilMap.ConvertListToEntity<IDto, TEntity>(dtos);
        }

        public static IEnumerable<TDto> ConvertListToDto<TDto>(this IEnumerable<object> entities)
            where TDto : class, new()
        {
            if (entities == null)
                return new List<TDto>();

            var utilMap = new GenericMap();

            var list = utilMap.MapList<object, TDto>(entities);
            return list;
        }

        public static TDto ConvertDataSetToDto<TDto>(this DataSet ds)
            where TDto : class, new()
        {
            if (ds == null)
                return new TDto();

            var utilMap = new GenericMap();

            return utilMap.MapObjectFromDataSet<TDto>(ds);
        }

        public static TDto ConvertDataTableToDto<TDto>(this DataTable dt)
            where TDto : class, new()
        {
            if (dt == null)
                return new TDto();

            var utilMap = new GenericMap();

            return utilMap.MapObjectFromDataTable<TDto>(dt);
        }

        public static TDto ConvertDataReaderToDto<TDto>(this IDataReader dr)
            where TDto : class, new()
        {
            if (dr == null)
                return new TDto();

            var utilMap = new GenericMap();

            return utilMap.MapObjectFromDataReader<TDto>(dr);
        }
       
        public static List<TDto> ConvertDataSetToListDto<TDto>(this DataSet ds)
           where TDto : class, new()
        {
            if (ds == null)
                return new List<TDto>();

            var utilMap = new GenericMap();

            return utilMap.MapListFromDataSet<TDto>(ds);
        }

        public static List<TDto> ConvertDataReaderToListDto<TDto>(this IDataReader dr)
            where TDto : class, new()
        {
            if (dr == null)
                return new List<TDto>();

            var utilMap = new GenericMap();

            return utilMap.MapListFromDataReader<TDto>(dr);
        }

        public static List<TDto> ConvertDataTableToListDto<TDto>(this DataTable dt)
            where TDto : class, new()
        {
            if (dt == null)
                return new List<TDto>();

            var utilMap = new GenericMap();

            return utilMap.MapListFromDataTable<TDto>(dt);
        }
    }
}