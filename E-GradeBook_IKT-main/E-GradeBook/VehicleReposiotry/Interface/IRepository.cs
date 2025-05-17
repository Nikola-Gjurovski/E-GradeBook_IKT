using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleReposiotry.Interface
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(Guid? id);
        T Get(Guid? id, params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties); // <-- Add this
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
