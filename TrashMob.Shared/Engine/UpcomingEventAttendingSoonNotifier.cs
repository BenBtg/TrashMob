﻿
namespace TrashMob.Shared.Engine
{
    using Microsoft.Extensions.Logging;
    using TrashMob.Shared.Persistence;

    public class UpcomingEventAttendingSoonNotifier : UpcomingEventAttendingBaseNotifier, INotificationEngine
    {
        protected override NotificationTypeEnum NotificationType => NotificationTypeEnum.UpcomingEventAttendingSoon;

        protected override int NumberOfHoursInWindow => 24;

        protected override string EmailSubject => "You're attending a TrashMob.eco event soon!";

        public UpcomingEventAttendingSoonNotifier(IEventRepository eventRepository,
                                                  IUserRepository userRepository, 
                                                  IEventAttendeeRepository eventAttendeeRepository,
                                                  IUserNotificationRepository userNotificationRepository,
                                                  INonEventUserNotificationRepository nonEventUserNotificationRepository,
                                                  IEmailSender emailSender,
                                                  IEmailManager emailManager,
                                                  IMapRepository mapRepository,
                                                  ILogger logger) :
            base(eventRepository, userRepository, eventAttendeeRepository, userNotificationRepository, nonEventUserNotificationRepository, emailSender, emailManager, mapRepository, logger)
        {
        }
    }
}
