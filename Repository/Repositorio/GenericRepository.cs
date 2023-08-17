using Microsoft.EntityFrameworkCore;
using Repository.Context;
using System.Linq.Expressions;
using System.Reflection;

namespace Repository.Repositorio
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDBContext context;
        private DbSet<TEntity> entities;

        private List<MemberInfo> members = new List<MemberInfo>();
        public GenericRepository(ApplicationDBContext context)
        {
            this.context = context;
            this.entities = context.Set<TEntity>();
        }
        public async Task<int> Crear(TEntity entity)
        {
            await entities.AddAsync(entity);
            var res = await context.SaveChangesAsync();
            return res;
        }

        public async Task<int> Crear(List<TEntity> entity)
        {
            await entities.AddRangeAsync(entity);
            return await context.SaveChangesAsync();
        }

        //public async Task<int> CrearMasivo(List<TEntity> entities)
        //{
        //    using (var transaction = context.Database.BeginTransaction())
        //    {
        //        var value = 0;
        //        try
        //        {
        //            await context.BulkInsertAsync(entities);
        //            transaction.Commit();
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.Write(e.Message);
        //            return value;
        //        }
        //        return value + 1;
        //    }
        //}

        public async Task<int> Eliminar(int? id)
        {
            var entity = await this.entities.FindAsync(id);
            entities.Remove(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Eliminar(TEntity entity)
        {
            entities.Remove(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> EliminarMasivo(List<TEntity> entities)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                var value = 0;
                try
                {
                    this.entities.RemoveRange(entities);
                    value = await context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    return value;
                }
                return value;
            }
        }

        public async Task<List<TEntity>> ObtieneLista()
        {
            return await entities.ToListAsync();
        }
        public async Task<List<TEntity>> ObtieneLista(Expression<Func<TEntity, bool>> lambda)
        {
            return await entities.Where(lambda).ToListAsync();
        }

        public async Task<List<TType>> ObtieneLista<TType>(Expression<Func<TEntity, TType>> select) where TType : class
        {
            return await entities.Select(select).ToListAsync();
        }
        public async Task<List<TEntity>> ObtieneLista(Expression<Func<TEntity, bool>> lambda, string relationships)
        {
            if (String.IsNullOrEmpty(relationships))
            {
                throw new Exception("No se definio las las relaciones a cargar.");
            }
            return await entities.AsQueryable().ObtieneRelaciones(relationships).Where(lambda).ToListAsync();
        }

        public async Task<List<TEntity>> ObtieneLista(string relationships)
        {
            if (String.IsNullOrEmpty(relationships))
            {
                throw new Exception("No se definio las las relaciones a cargar.");
            }
            return await entities.AsQueryable().ObtieneRelaciones(relationships).ToListAsync();
        }

        public async Task<List<TType>> ObtieneLista<TType>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TType>> select) where TType : class
        {
            return await entities.Where(lambda).Select(select).ToListAsync();
        }

        public async Task<List<TType>> ObtieneLista<TType>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TType>> select, string relationships) where TType : class
        {
            return await entities.AsQueryable().ObtieneRelaciones(relationships).Where(lambda).Select(select).ToListAsync();
        }

        public async Task<List<TType>> ObtieneListaIgnoreFilter<TType>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TType>> select, string relationships) where TType : class
        {
            return await entities.IgnoreQueryFilters().AsQueryable().ObtieneRelaciones(relationships).Where(lambda).Select(select).ToListAsync();
        }
        public async Task<List<TType>> ObtieneLista<TType>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TType>> select, string relationships, string relationships2) where TType : class
        {
            return await entities.AsQueryable().ObtieneRelaciones(relationships).ObtieneRelaciones(relationships2).Where(lambda).Select(select).ToListAsync();
        }

        public async Task<List<TEntity>> ObtieneListaDesc<TKey>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TKey>> orderByDescending, int registros = 0)
        {
            var query = entities.Where(lambda).OrderByDescending(orderByDescending);

            if (registros > 0)
            {
                return await query.Take(registros).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<TEntity> BuscarPorId(int? id)
        {
            return await entities.FindAsync(id);
        }

        public async Task<TEntity> BuscarUnElemento(Expression<Func<TEntity, bool>> lambda)
        {
            return await entities.FirstOrDefaultAsync(lambda);
        }
        public async Task<TType> BuscarUnElemento<TType>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TType>> select) where TType : class
        {
            return await entities.AsQueryable().Where(lambda).Select(select).FirstOrDefaultAsync();
        }

        public async Task<TEntity> BuscarUnElemento(Expression<Func<TEntity, bool>> lambda, string relationships)
        {
            if (String.IsNullOrEmpty(relationships))
            {
                throw new Exception("No se definio las las relaciones a cargar.");
            }
            return await entities.AsQueryable().ObtieneRelaciones(relationships).FirstOrDefaultAsync(lambda);
        }
        public async Task<TEntity> BuscarUnElementoIgnoreFilter(Expression<Func<TEntity, bool>> lambda, string relationships)
        {
            if (String.IsNullOrEmpty(relationships))
            {
                throw new Exception("No se definio las las relaciones a cargar.");
            }
            return await entities.IgnoreQueryFilters().AsQueryable().ObtieneRelaciones(relationships).FirstOrDefaultAsync(lambda);
        }
        public async Task<List<TEntity>> BuscarUnListaIgnoreFilter(Expression<Func<TEntity, bool>> lambda, string relationships)
        {
            if (String.IsNullOrEmpty(relationships))
            {
                throw new Exception("No se definio las las relaciones a cargar.");
            }
            return await entities.IgnoreQueryFilters().AsQueryable().ObtieneRelaciones(relationships).Where(lambda).ToListAsync();
        }
        public async Task<TType> BuscarUnElemento<TType>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TType>> select, string relaciones) where TType : class
        {
            if (String.IsNullOrEmpty(relaciones))
            {
                throw new Exception("No se definio las las relaciones a cargar.");
            }

            return await entities.AsQueryable().ObtieneRelaciones(relaciones)
                .Where(lambda).Select(select).FirstOrDefaultAsync();
        }


        public async Task<int> Actualizar(TEntity entity)
        {
            entities.Update(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Actualizar(List<TEntity> entity)
        {
            entities.UpdateRange(entity);
            return await context.SaveChangesAsync();
        }
        public async Task<List<TType>> ObtieneListaStock<TType>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TType>> select, string relationships) where TType : class
        {
            return await entities.AsQueryable().ObtieneRelaciones(relationships).Where(lambda).Select(select).ToListAsync();
        }

        public async Task<TEntity> BuscarObjetoNombre(Expression<Func<TEntity, bool>> lambda)
        {
            return await entities.Where(lambda).FirstOrDefaultAsync();
        }
        public async Task<bool> ExisteElemento(Expression<Func<TEntity, bool>> lambda)
        {
            return await entities.AsQueryable().AnyAsync(lambda);
        }
    }

    public static class GenericRepositoryExtensions
    {
        public static IQueryable<TEntity> ObtieneRelaciones<TEntity>(this IQueryable<TEntity> entidades, string includedProperties) where TEntity : class
        {
            // var entidades = this.entities.AsQueryable();
            var relations = includedProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var property in relations)
            {
                entidades = entidades.Include(property);
            }

            return entidades;
        }

    }
}
