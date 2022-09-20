﻿
namespace TrashMob.Shared.Engine
{
    using Microsoft.Extensions.Logging;
    using TrashMob.Shared.Persistence.Interfaces;

    public class UpcomingEventHostingThisWeekNotifier : UpcomingEventHostingBaseNotifier, INotificationEngine
    {
        protected override NotificationTypeEnum NotificationType => NotificationTypeEnum.UpcomingEventHostingThisWeek;

        protected override int NumberOfHoursInWindow => 7 * 24;

        protected override string EmailSubject => "You're hosting a TrashMob.eco event this week!";

        public UpcomingEventHostingThisWeekNotifier(IEventRepository eventRepository, 
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
