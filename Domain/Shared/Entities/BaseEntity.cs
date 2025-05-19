using Domain.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        private List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
