﻿namespace TrashMob.Shared.Engine
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TrashMob.Models;
    using TrashMob.Models.Extensions;
    using TrashMob.Shared.Extensions;
    using TrashMob.Shared.Managers.Interfaces;
    using TrashMob.Shared.Persistence.Interfaces;

    public abstract class NotificationEngineBase
    {
        protected IEventManager EventManager { get; }

        protected IKeyedManager<User> UserManager { get; }

        protected IEventAttendeeManager EventAttendeeManager { get; }

        protected IKeyedManager<UserNotification> UserNotificationManager { get; }

        protected IKeyedManager<NonEventUserNotification> NonEventUserNotificationManager { get; }

        protected IEmailManager EmailManager { get; }

        protected IEmailSender EmailSender { get; }

        protected IMapManager MapRepository { get; }

        public ILogger Logger { get; }

        protected abstract NotificationTypeEnum NotificationType { get; }

        protected abstract int NumberOfHoursInWindow { get; }

        protected abstract string EmailSubject { get; }

        protected string SendGridApiKey { get; }

        public NotificationEngineBase(IEventManager eventManager,
                                      IKeyedManager<User> userManager,
                                      IEventAttendeeManager eventAttendeeManager,
                                      IKeyedManager<UserNotification> userNotificationManager,
                                      IKeyedManager<NonEventUserNotification> nonEventUserNotificationManager,
                                      IEmailSender emailSender,
                                      IEmailManager emailManager,
                                      IMapManager mapRepository,
                                      ILogger logger)
        {
            EventManager = eventManager;
            UserManager = userManager;
            EventAttendeeManager = eventAttendeeManager;
            UserNotificationManager = userNotificationManager;
            EmailSender = emailSender;
            EmailManager = emailManager;
            MapRepository = mapRepository;
            NonEventUserNotificationManager = nonEventUserNotificationManager;
            Logger = logger;

            // Set the Api Key Here
            EmailSender.ApiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
        }

        protected async Task<int> SendNotifications(User user, IEnumerable<Event> eventsToNotifyUserFor, CancellationToken cancellationToken)
        {
            // Populate email
            if (eventsToNotifyUserFor.Any())
            {
                // Update the database first so that a user is not notified multiple times
                foreach (var mobEvent in eventsToNotifyUserFor)
                {
                    var userNotification = new UserNotification
                    {
                        Id = Guid.NewGuid(),
                        EventId = mobEvent.Id,
                        UserId = user.Id,
                        SentDate = DateTimeOffset.UtcNow,
                        UserNotificationTypeId = (int)NotificationType,
                    };

                    await UserNotificationManager.Add(userNotification).ConfigureAwait(false);
                }

                var emailCopy = EmailManager.GetHtmlEmailCopy(NotificationType.ToString());

                foreach (var mobEvent in eventsToNotifyUserFor)
                {
                    Logger.LogInformation("Getting local event time for eventId: {eventId}", mobEvent.Id);

                    var localDate = await mobEvent.GetLocalEventTime(MapRepository).ConfigureAwait(false);

                    Logger.LogInformation("UTC event time for eventId ({eventId}): {eventDate}.", mobEvent.Id, mobEvent.EventDate);
                    Logger.LogInformation("Local event time for eventId ({eventId}): {localDate}.", mobEvent.Id, localDate);

                    // If the email has an event summary, add it here.
                    emailCopy = emailCopy.Replace("{eventSummaryUrl}", mobEvent.EventSummaryUrl());

                    var dynamicTemplateData = new
                    {
                        username = user.UserName,
                        eventName = mobEvent.Name,
                        eventDate = localDate.Item1,
                        eventTime = localDate.Item2,
                        eventAddress = mobEvent.EventAddress(),
                        emailCopy = emailCopy,
                        subject = EmailSubject,
                        eventDetailsUrl = mobEvent.EventDetailsUrl(),
                        eventSummaryUrl = mobEvent.EventSummaryUrl(),
                        googleMapsUrl = mobEvent.GoogleMapsUrl(),
                    };

                    var recipients = new List<EmailAddress>
                        {
                            new EmailAddress { Name = user.UserName, Email = user.Email },
                        };

                    Logger.LogInformation("Sending email to {0}, Subject {0}", user.Email, EmailSubject);

                    await EmailManager.SendTemplatedEmail(EmailSubject, SendGridEmailTemplateId.EventEmail, SendGridEmailGroupId.EventRelated, dynamicTemplateData, recipients, CancellationToken.None).ConfigureAwait(false);
                }

                return 1;
            }

            return 0;
        }

        protected async Task<int> SendNotification(User user, CancellationToken cancellationToken)
        {
            // Populate email
            var userNotification = new NonEventUserNotification
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                SentDate = DateTimeOffset.UtcNow,
                UserNotificationTypeId = (int)NotificationType,
            };

            await NonEventUserNotificationManager.Add(userNotification).ConfigureAwait(false);

            var emailCopy = EmailManager.GetHtmlEmailCopy(NotificationType.ToString());

            var dynamicTemplateData = new
            {
                username = user.UserName,
                emailCopy = emailCopy,
                subject = EmailSubject,
            };

            var recipients = new List<EmailAddress>
                        {
                            new EmailAddress { Name = user.UserName, Email = user.Email },
                        };

            Logger.LogInformation("Sending email to {0}, Subject {0}", user.Email, EmailSubject);

            await EmailManager.SendTemplatedEmail(EmailSubject, SendGridEmailTemplateId.GenericEmail, SendGridEmailGroupId.General, dynamicTemplateData, recipients, CancellationToken.None).ConfigureAwait(false);

            return 1;
        }

        protected async Task<bool> UserHasAlreadyReceivedNotification(User user, Event mobEvent, CancellationToken cancellationToken)
        {
            // Get list of notification events user has already received for the event
            var notifications = await UserNotificationManager.Get(u => u.UserId == user.Id && u.EventId == mobEvent.Id, cancellationToken).ConfigureAwait(false);

            // Verify that the user has not already received this type of notification for this event
            return notifications.Any(un => un.UserNotificationTypeId == (int)NotificationType);
        }

        protected async Task<bool> UserHasAlreadyReceivedNotification(User user, CancellationToken cancellationToken)
        {
            // Get list of notification events user has already received for the event
            var notifications = await NonEventUserNotificationManager.GetByUserId(user.Id, cancellationToken).ConfigureAwait(false);

            // Verify that the user has not already received this type of notification for this event
            return notifications.Any(un => un.UserNotificationTypeId == (int)NotificationType);
        }
    }
}
