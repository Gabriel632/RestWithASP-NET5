using Microsoft.EntityFrameworkCore;
using RestWithASPNET5.Model.Base;
using RestWithASPNET5.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNET5.Repository.Generic
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly MySQLContext _context;
        private DbSet<T> dataset;

        public GenericRepository(MySQLContext mySQLContext)
        {
            _context = mySQLContext;
            dataset = _context.Set<T>();
        }


        public T Create(T item)
        {
            try
            {
                dataset.Add(item);
                _context.SaveChanges();

                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(long id)
        {
            var result = dataset.SingleOrDefault(p => p.Id.Equals(id));
            if (result == null) return;

            try
            {
                dataset.Remove(result);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Exists(long id)
        {
            return dataset.Any(p => p.Id.Equals(id));
        }

        public List<T> FindAll()
        {
            return dataset.ToList();
        }

        public T FindByID(long id)
        {
            return dataset.SingleOrDefault(p => p.Id.Equals(id));
        }

        public List<T> FindWithPagedSearch(string query)
        {
            return dataset.FromSqlRaw<T>(query).ToList();
        }

        public int GetCount(string query)
        {
            var result = "";

            using (var connection = _context.Database.GetDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    result = command.ExecuteScalar().ToString();
                }
            }

            return int.Parse(result);
        }

        public T Update(T item)
        {
            if (!Exists(item.Id)) return null;

            var result = dataset.SingleOrDefault(p => p.Id.Equals(item.Id));
            if (result == null) return null;

            try
            {
                _context.Entry(result).CurrentValues.SetValues(item);
                _context.SaveChanges();

                return result;
            }
            catch (Exception)
            {
                throw;
            }            
        }
    }
}
