using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace GenericMapper.Interfaces
{
    public interface IGenericMap
    {
        void MapObject<TEntity, TDto>(TEntity entity, TDto dto)
            where TEntity : class
            where TDto : class;

        TDto MapObject<TEntity, TDto>(TEntity entity)
            where TEntity : class
            where TDto : class, new();

        IEnumerable<TDto> MapList<TEntity, TDto>(IEnumerable<TEntity> entities)
            where TEntity : class
            where TDto : class, new();

        void MapList<TEntity, TDto>(IEnumerable<TEntity> entities, ref IEnumerable<TDto> dtos)
            where TEntity : class
            where TDto : class, new();

        void MapList<TEntity, TDto>(IEnumerable<TEntity> entities, ref List<TDto> dtos)
            where TEntity : class
            where TDto : class, new();

        void ConvertToEntity<TDto, TEntity>(TDto dto, ref TEntity entity) 
            where TDto : class
            where TEntity : class, new();

        TEntity ConvertToEntity<TDto, TEntity>(TDto dto)
            where TDto : class
            where TEntity : class, new();

        IEnumerable<TEntity> ConvertListToEntity<TDto, TEntity>(IEnumerable<TDto> dtos)
            where TDto : class
            where TEntity : class, new();

        void ConvertListToEntity<TDto, TEntity>(IEnumerable<TDto> dtos, ref IEnumerable<TEntity> entities)
            where TDto : class
            where TEntity : class, new();

        void ConvertListToEntity<TDto, TEntity>(IEnumerable<TDto> dtos, ref List<TEntity> entities)
            where TDto : class
            where TEntity : class, new();

        List<TDTO> MapListFromDataSet<TDTO>(DataSet ds)
            where TDTO : class, new();

        TDTO MapObjectFromDataSet<TDTO>(DataSet ds)
            where TDTO : class, new();

        List<TDTO> MapListFromDataTable<TDTO>(DataTable table)
            where TDTO : class, new();

        TDTO MapObjectFromDataTable<TDTO>(DataTable table)
            where TDTO : class, new();

        List<TDTO> MapListFromDataReader<TDTO>(IDataReader reader)
            where TDTO : class, new();

        TDTO MapObjectFromDataReader<TDTO>(IDataReader reader)
            where TDTO : class, new();
    }
}