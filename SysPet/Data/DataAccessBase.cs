﻿namespace SysPet.Data
{
    public abstract class DataAccessBase<T>
    {
        private readonly DataAccess<T> data;

        protected DataAccessBase()
        {
            data = new DataAccess<T>();  
        }

        public abstract Task<IEnumerable<T>> GetAll();
        public abstract Task<T> GetItem(int id);
        public abstract int Create(T item);
        public abstract int Update(T item, int id);
        public abstract int Delete(int id);

        public int Execute(string sql)
        {
            return data.Execute(sql);
        }

        public async Task<IEnumerable<T>> GetItems(string sql)
        {
            return await data.GetAll(sql);
        }

        public async Task<T> Get(string sql, object param)
        {
            return await data.Get(sql, param);
        }

        public int Execute(string sql, object param)
        {
            return data.Execute(sql, param);
        }

        public string FormatDate(DateTime date)
        {
            return $"{date.Year}-{date.Month}-{date.Day}";
        }

        public string FormatDateTime(DateTime date)
        {
            return $"{date.Year}-{date.Month}-{date.Day} {date.Hour}:{date.Minute}";
        }

        public static int GetEstado(bool estado)
        {
            return estado ? 1 : 0;
        }
    }
}