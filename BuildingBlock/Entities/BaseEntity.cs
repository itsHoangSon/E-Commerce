using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        private List<INotification> _domainEvents;
        public List<INotification> DomainEvents => _domainEvents;
        public long Id { get; set; }

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
    public interface IBaseEntity
    {
        long Id { get; set; }
    }
}
