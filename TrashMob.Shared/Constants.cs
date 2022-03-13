﻿namespace TrashMob.Shared
{
    public static class Constants
    {
        public const string TrashMobReadScope = "TrashMob.Read";
        public const string TrashMobWriteScope = "TrashMob.Writes";
        public const string TrashMobEmailAddress = "info@trashmob.eco";
        public const string TrashMobEmailName = "TrashMob Information";
    }

    public enum EventStatusEnum
    {
        Active = 1,
        Full = 2,
        Canceled = 3,
        Complete = 4,
    }

    public enum PartnerRequestStatusEnum
    {
        Pending = 1,
        Approved = 2,
        Denied = 3,
    }

    public enum PartnerStatusEnum
    {
        Active = 1,
        Inactive = 2,
    }

    public enum EventPartnerStatusEnum
    {
        None = 0,
        Requested = 1,
        Accepted = 2,
        Declined = 3,
    }

    public enum WaiverDurationTypeEnum
    {
        CalendarYear = 1,
        YearFromSigning = 2,
        MonthFromSigning = 3,
        SingleDay = 4,
    }
}
