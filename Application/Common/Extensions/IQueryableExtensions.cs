using Microsoft.EntityFrameworkCore;

namespace Application;

public static class DbSetExtensions
{
    public static async Task<IReadOnlyList<T>> ToPagedListAsync<T>(this IQueryable<T> queryable, int pageNumber, int pageSize) where T : class
    {
        return await queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }
}