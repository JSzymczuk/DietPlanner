using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Contract
{
    public interface IBaseManager<EntityType, IdType> : IDisposable
    {
        IQueryable<EntityType> Entities { get; }
        EntityType FindById(IdType id);
        void Create(EntityType element);
        void Delete(IdType elementId);
        void Update(EntityType element);
        void Save();
    }
}
