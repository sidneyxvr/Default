using Default.Business.Interfaces.Repositories;
using Default.Business.Models;
using Default.Business.Models.Views;
using Default.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace SGEM.Data.Repositories
{
    /// <summary>
    /// Gerenic repository that contains main methods can be used by specialized repositories
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly DefaultContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected Repository(DefaultContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Add a entity async
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>register added</returns>
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Remove a entity async
        /// </summary>
        /// <param name="entity"></param>
        public async Task RemoveAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        /// <summary>
        /// Check if entity exists by an id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>if entity exists or not</returns>
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbSet.AnyAsync(s => s.Id == id);
        }


        /// <summary>
        /// Generic filter
        /// <example>
        ///     filter: field1=str1;field2<=num1;field3==num2
        ///     one equal operator can be used by type string only
        /// </example>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <returns>query filtered out</returns>
        protected virtual IQueryable<TEntity> FilterOut(IQueryable<TEntity> query, string filter)
        {
            var operators = new[] { "==", "!=", "<", ">", "<=", ">=", "=" };
            var filterList = filter.Split(';', System.StringSplitOptions.RemoveEmptyEntries);
            if (filterList == null || filterList.Length == 0)
            {
                return query;
            }
            foreach (var filterBy in filterList)
            {
                var expr = filterBy.Trim().Split(operators, System.StringSplitOptions.RemoveEmptyEntries);
                var oper = filterBy.Trim().ElementAt(expr[0].Length);

                query = query.Where(BuildPredicate(expr[0], oper.ToString(), expr[1]));
            }

            return query;
        }

        /// <summary>
        /// Get all registers
        /// </summary>
        /// <returns>all registers</returns>
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get a register by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a register</returns>
        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// Seach a specific word in all string parameters
        /// </summary>
        /// <param name="query"></param>
        /// <param name="search"></param>
        /// <returns>query filtered out</returns>
        protected virtual IQueryable<TEntity> Search(IQueryable<TEntity> query, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return query;
            }

            ///Get all string properties
            var properties = typeof(TEntity).GetProperties().Where(a => a.PropertyType == typeof(string)).ToList();

            ///Create a parameter to use in a lambda
            var parameter = Expression.Parameter(typeof(TEntity));

            var expressions = new List<Expression>();

            foreach (var property in properties)
            {
                var left = property.Name.Split('.').Aggregate((Expression)parameter, Expression.PropertyOrField);
                var body = MakeComparison(left, "=", search.Trim());
                expressions.Add(body);
            }

            if (!expressions.Any())
            {
                return query;
            }

            ///Construct a lambda expression
            ///result: a => a.field1.Contains("value1") || a.field2.equal(value2)
            var expression = expressions.First();
            for (int i = 1; i < expressions.Count; i++)
            {
                expression = Expression.OrElse(expression, expressions.Last());
            }
            var lambda = Expression.Lambda<Func<TEntity, bool>>(expression, parameter);

            return query.Where(lambda);
        }

        /// <summary>
        /// Get a paged list
        /// </summary>
        /// <param name="page">integer</param>
        /// <param name="pageSize">interger</param>
        /// <param name="search">a word to be search</param>
        /// <param name="filter">string with patter: field1=value1;field2<=value2</param>
        /// <param name="order">string with patter: field1 asc</param>
        /// <returns>paged list that contains a collection paged and total amount</returns>
        public virtual async Task<PagedList<TEntity>> GetPagedAsync(int page = 0, 
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var query = _dbSet.AsNoTracking();
            query = FilterOut(query, filter);
            query = Search(query, search);
            var ordered = Order(query, order);
            ordered = Page(ordered, page, pageSize);
            return new PagedList<TEntity>
            {
                Collection = await ordered.ToListAsync(),
                Amount = await query.CountAsync()
            };
        }

        /// <summary>
        /// Order by a field and direction
        /// <example>
        ///     order: "field1 asc"
        /// </example>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <returns>query ordered</returns>
        protected virtual IQueryable<TEntity> Order(IQueryable<TEntity> query, string order = "")
        {
            var orderProperties = order.Split(' ');
            var orderBy = orderProperties.FirstOrDefault();
            var orderDirection = orderProperties.LastOrDefault();

            var property = typeof(TEntity).GetProperty(orderBy, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property == null || (!orderDirection.Equals("asc") && !orderDirection.Equals("desc")))
            {
                return query;
            }

            return query.OrderBy($"{orderBy} {orderDirection}");
        }

        /// <summary>
        /// Page a query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>query paged</returns>
        protected IQueryable<TEntity> Page(IQueryable<TEntity> query, int page = 0, int pageSize = 10)
        {
            if (page == 0)
            {
                return query.Take(1000);
            }
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }


        /// <summary>
        /// Commit changes on database
        /// </summary>
        protected async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update a register async
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>register updated</returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            _context.Entry(entity).Property(a => a.Enable).IsModified = false;
            _context.Entry(entity).Property(a => a.Removed).IsModified = false;
            _context.Entry(entity).Property(a => a.CreationDate).IsModified = false;
            _context.Entry(entity).Property(a => a.CreatedById).IsModified = false;
            await SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Add a list of registers
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns>a list of registers added</returns>
        public async Task<List<TEntity>> AddAsync(List<TEntity> entityList)
        {
            await _dbSet.AddRangeAsync(entityList);
            await SaveChangesAsync();
            return entityList;
        }

        /// <summary>
        /// Build a lambda expression
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="comparison">
        ///     "==", "!=", "<", ">", "<=", ">=", "="
        /// </param>
        /// <param name="value"></param>
        /// <returns>lambda expression</returns>
        public Expression<Func<TEntity, bool>> BuildPredicate(string propertyName, string comparison, string value)
        {
            var parameter = Expression.Parameter(typeof(TEntity));
            var left = propertyName.Split('.').Aggregate((Expression)parameter, Expression.PropertyOrField);
            var body = MakeComparison(left, comparison, value);
            return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
        }

        /// <summary>
        /// Get a expression to a specific comparison
        /// </summary>
        /// <param name="left"></param>
        /// <param name="comparison"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private Expression MakeComparison(Expression left, string comparison, string value)
        {
            object typedValue = value;

            var valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
            typedValue = valueType.IsEnum ? Enum.Parse(valueType, value) :
                valueType == typeof(Guid) ? Guid.Parse(value) :
                Convert.ChangeType(value, valueType);

            var constant = Expression.Constant(typedValue);
            switch (comparison)
            {
                case "==":
                    return Expression.Equal(left, constant);
                case "!=":
                    return Expression.NotEqual(left, constant);
                case ">":
                    return Expression.GreaterThan(left, constant);
                case ">=":
                    return Expression.GreaterThanOrEqual(left, constant);
                case "<":
                    return Expression.LessThan(left, constant);
                case "<=":
                    return Expression.LessThanOrEqual(left, constant);
                case "=":
                    if (value is string)
                    {
                        return Expression.Call(left, "Contains", Type.EmptyTypes, Expression.Constant(value, typeof(string)));
                    }
                    throw new NotSupportedException($"Comparison operator '{comparison}' only supported on string.");

                default:
                    throw new NotSupportedException($"Invalid comparison operator '{comparison}'.");
            }
        }


        /// <summary>
        /// Include multiples joins on a query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="includes">
        ///     for this: Include(a => a.Field1).ThenInclude(b => b.Field2) ...
        ///     use this: "Field1.Field2" ...
        /// </param>
        /// <returns>a query with joins included</returns>
        protected IQueryable<TEntity> IncludeMultiple(IQueryable<TEntity> query, params string[] includes)
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }
    }
}
