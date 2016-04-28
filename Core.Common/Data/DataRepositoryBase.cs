using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Utils;

namespace Core.Common.Data
{
    public abstract class DataRepositoryBase<T, U> : IDataRepository<T>
        where T : class, IIdentifiableEntity, new()
        where U : DbContext, new()
    {
        protected abstract T AddEntity(U entityContext, T entity);

        protected abstract T UpdateEntity(U entityContext, T entity);

        protected abstract IEnumerable<T> GetEntities(U entityContext);

        protected abstract T GetEntity(U entityContext, int id);


        public T Add(T entity)
        {
            using (U entityConxtext = new U())
            {
                T addedEntity = AddEntity(entityConxtext, entity);
                entityConxtext.SaveChanges();
                return addedEntity;
            }
        }

        public void Remove(T entity)
        {
            using (U entityConxtext = new U())
            {
                entityConxtext.Entry<T>(entity).State = EntityState.Deleted;
                entityConxtext.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            using (U entityConxtext = new U())
            {
                T entity = GetEntity(entityConxtext, id);
                entityConxtext.Entry<T>(entity).State = EntityState.Deleted;
                entityConxtext.SaveChanges();
            }
        }

        public T Update(T entity)
        {
            using (U entityConxtext = new U())
            {
                T existingEntity = UpdateEntity(entityConxtext, entity);

                SimpleMapper.PropertyMap(entity, existingEntity);

                entityConxtext.SaveChanges();
                return existingEntity;

            }
        }

        public IEnumerable<T> Get()
        {
            using (U entityContext = new U())
            {
                return (GetEntities(entityContext)).ToArray().ToList();
            }
        }

        public T Get(int id)
        {
            using (U entityContext = new U())
            {
                return (GetEntity(entityContext, id));
            }
        }
    }
}
