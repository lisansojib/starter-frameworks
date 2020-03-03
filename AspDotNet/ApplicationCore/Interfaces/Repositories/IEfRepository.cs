﻿using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IEfRepository<T> where T: IBaseEntity
    {
        T Find(int id);
        T Find(Expression<Func<T, bool>> criteria);
        bool Exists(int id);
        int Count();
        int GetMaxId();
        IQueryable<T> ListAll();
        List<T> ListAll(Expression<Func<T, bool>> criteria);
        List<T> ListAll(int page, int pageSize);
        List<T> ListAll(int offset, int limit, FilterByExpression FilterByExpression, string sort, string order, out int count);
        List<T> ListAll(Expression<Func<T, bool>> criteria, int offset, int limit, FilterByExpression FilterByExpression, string sort, string order, out int count);
        IQueryable<T> List(Expression<Func<T, bool>> criteria);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<T> FindAsync(int id);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria);
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync();
        Task<List<T>> ListAllAsync();
        Task<List<T>> ListAllAsync(int page, int pageSize);
        Task<List<T>> ListAllAsync(int offset, int limit, FilterByExpression FilterByExpression, string sort, string order);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
