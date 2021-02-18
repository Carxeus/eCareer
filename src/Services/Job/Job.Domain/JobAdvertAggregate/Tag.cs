using System;
using Career.Domain.Entities;

namespace Job.Domain.JobAdvertAggregate
{
    public class Tag : DomainEntity
    {
        public Tag()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; }
        public string Name { get; set; }
    }
}