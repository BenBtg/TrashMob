﻿namespace TrashMobMobileApp.Features.Map
{
    using Microsoft.Maui.Controls.Maps;
    using TrashMob.Models;

    internal static class MapHelper
    {
        public static Pin GetPinForEvent(Event mobEvent)
        {
            var eventHeader = string.Format("{0}: {1:yyyy-MM-dd HH:mm}", mobEvent.Name, mobEvent.EventDate);

            var pin = new Pin
            {
                Label = eventHeader,
                Address = mobEvent.StreetAddress,
                Type = PinType.Place
            };

            return pin;
        }

        public static Pin GetPinForUser(User user)
        {
            var userHeader = user.UserName;

            var pin = new Pin
            {
                Label = userHeader,
                Address = string.Format("{city}, {region}", user.City, user.Region),
                Type = PinType.Place
            };

            return pin;
        }
    }
}
