using System;
using System.Linq;
using System.Threading.Tasks;
using AlintaDomain;
using Microsoft.EntityFrameworkCore;

namespace AlintaEF
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly AlintaEFContext Context;
        protected readonly DbSet<TEntity> Table;

        public Repository(AlintaEFContext context)
        {
            Context = context;
            Table = Context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return Context.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public async Task<TEntity> Get(object id)
        {
            TEntity existing = await Table.FindAsync(id);
            return existing;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                await Context.AddAsync(entity);
                await Context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                Context.Update(entity);
                await Context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException($"{nameof(DeleteAsync)} entity id must not be null");
            }
            TEntity existing = await Table.FindAsync(id);
            if (existing == null)
            {
                return false;
            }

            Table.Remove(existing);
            return true;
        }
    }
}