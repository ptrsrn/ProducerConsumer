using System;

namespace DataLayer
{
    public class Repository<TEntry> : IRepository<TEntry> where TEntry: class, new()
    {
        private readonly MessagingContext context;

        public Repository(MessagingContext context)
        {
            this.context = context;
        }

        public TEntry Add(TEntry entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity must not be null");
            }

            try
            {

                this.context.Add(entity);
                this.context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }
    }


}