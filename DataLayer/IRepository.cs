namespace DataLayer
{
    public interface IRepository<TEntity> where TEntity: class, new()
    {   
        TEntity Add(TEntity entity);
    }


}