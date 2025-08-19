using BuildingBlock.MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Paginated
{
    public static class Extensions
    {
        public static List<T> ToPaged<T>(this IQueryable<T> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public static async Task<List<T>> ToPagedAsync<T>(this IQueryable<T> source, int page, int pageSize)
        {
            return await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public static async Task<List<T>> ToPagedAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            return await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        }

        public static async Task<List<T>> ToPagedAsync<T>(this IQueryable<T> source, IPaginatedQuery query, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(query.SortBy))
            {
                return await source.Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize).ToListAsync(cancellationToken);
            }
            else
            {
                if (query.SortType == SortType.Desc)
                {
                    return await source.OrderByDescending(e => EF.Property<T>(e, query.SortBy))
                         .Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize).ToListAsync(cancellationToken);
                }
                else
                {
                    return await source.OrderBy(e => EF.Property<T>(e, query.SortBy))
                        .Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize).ToListAsync(cancellationToken);
                }
            }
        }

        public static async Task<PaginatedList<TOut>> ToPagedAsync<TEntity, TOut>(this IQueryable<TEntity> source, IPaginatedQuery query, CancellationToken cancellationToken = default)
        {
            var items = default(List<TEntity>);

            if (string.IsNullOrWhiteSpace(query.SortBy))
            {
                items = await source.OrderBy(o => EF.Property<TEntity>(o, "Id")).Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize).ToListAsync(cancellationToken);
            }
            else
            {
                if (query.SortType == SortType.Desc)
                {
                    items = await source.OrderByDescending(e => EF.Property<TEntity>(e, query.SortBy))
                        .Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize).ToListAsync(cancellationToken);
                }
                else
                {
                    items = await source.OrderBy(e => EF.Property<TEntity>(e, query.SortBy))
                        .Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize).ToListAsync(cancellationToken);
                }
            }

            return new PaginatedList<TOut>(items.MapTo<List<TOut>>(), source.Count(), query.PageIndex, query.PageSize);
        }

        public static IQueryable<T> ToPagedQuery<T>(this IQueryable<T> source, IPaginatedQuery query)
        {
            return source.Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize);
        }
    }
}
