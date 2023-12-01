using Hamburguesas.Models.Entities;

namespace Hamburguesas.Repositories
{
    public class Repository<T> where T : class
    {
        public Repository(NeatContext context)
        {
            Context = context;
        }

        public NeatContext Context { get; }
        public virtual T? Get(object id)
        {
            return Context.Find<T>(id);
        }
        public virtual void Insert(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }
        public virtual void Update(T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }
        public virtual void Delete(T entity)
        {
            Context.Remove(entity);
            Context.SaveChanges();
        }
        public virtual void Delete(object id)
        {
            var enity = Get(id);
            if (enity != null)
            {
                Delete(enity);
            }
        }
    }
}
