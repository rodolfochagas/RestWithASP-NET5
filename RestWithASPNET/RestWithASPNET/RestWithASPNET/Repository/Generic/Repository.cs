using Microsoft.EntityFrameworkCore;
using RestWithASPNET.Model.Base;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNET.Repository.Generic
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected MySQLContext _context;
        private DbSet<T> dataset;

        public Repository(MySQLContext context)
        {
            _context = context;
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
            var result = FindById(id);

            if (result != null)
            {
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
        }

        public bool Exists(long id)
        {
            return dataset.Any(p => p.Id.Equals(id));
        }

        public List<T> FindAll()
        {
            return dataset.ToList();
        }

        public T FindById(long id)
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
            var result = FindById(item.Id);

            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(item);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }

            }
            return item;
        }
    }
}
