using System.Linq.Expressions;


namespace Repository.Repositorio
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> ObtieneLista();
        Task<List<TEntity>> ObtieneLista(Expression<Func<TEntity, bool>> lambda);
        Task<List<TType>> ObtieneLista<TType>(Expression<Func<TEntity, TType>> select) where TType : class;
        Task<List<TEntity>> ObtieneLista(Expression<Func<TEntity, bool>> lambda, string relationships);
        Task<List<TType>> ObtieneLista<TType>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TType>> select) where TType : class;
        Task<List<TType>> ObtieneLista<TType>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TType>> select, string relationships) where TType : class;
        Task<TEntity> BuscarPorId(int? id);
        Task<TEntity> BuscarUnElemento(Expression<Func<TEntity, bool>> lambda);
        Task<bool> ExisteElemento(Expression<Func<TEntity, bool>> lambda);
        Task<TType> BuscarUnElemento<TType>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TType>> select) where TType : class;
        Task<TEntity> BuscarUnElemento(Expression<Func<TEntity, bool>> lambda, string relationships);
        Task<TType> BuscarUnElemento<TType>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TType>> select, string relaciones) where TType : class;
        Task<int> Crear(TEntity entity);
        Task<int> Actualizar(TEntity entity);
        Task<int> Eliminar(int? id);
    }
}
