using System;
using System.Collections.Generic;
using Career.Domain;
using Career.Domain.Entities;
using Career.Exceptions;
using Career.Shared.Timing;
using Job.Domain.JobAdvertAggregate;
using Job.Domain.JobApplicationAggregate.DomainEvents;

namespace Job.Domain.JobApplicationAggregate
{
    public class JobApplication : DomainEntity, IAggregateRoot
    {
        public JobApplication()
        {
            Id = Guid.NewGuid();
            IsActive = true;
        }

        public Guid Id { get; }
        public Guid JobAdvertId { get; private init; }
        public Guid UserId { get; private init; }
        public Guid CvId { get; private init; }
        public string CoverLetter { get; private init; }
        public string Channel { get; private init; }
        public string Referance { get; private init; }
        public DateTime ApplicationDate { get; private init; }
        public DateTime? WithdrawalDate { get; private set; }
        public bool IsActive { get; private set; }

        public static JobApplication Create(JobAdvert jobAdvert, Guid userId, Guid cvId, string coverLetter, string channel, string referance)
        {
            Check.NotNull(jobAdvert, nameof(jobAdvert));
            Check.NotEmpty(userId, nameof(userId));
            Check.NotEmpty(cvId, nameof(cvId));

            JobApplication application = new()
            {
                JobAdvertId = jobAdvert.Id,
                UserId = userId,
                CvId = cvId,
                Channel = channel,
                Referance = referance,
                CoverLetter = coverLetter,
                ApplicationDate = Clock.Now
            };
            
            application.AddDomainEvent(new ApplicationCreatedEvent(jobAdvert, application, ApplicationEventSource.JobApplication));
            
            return application;
        }

        public void Withdrawn()
        {
            IsActive = false;
            WithdrawalDate = Clock.Now;
        }
    }
}