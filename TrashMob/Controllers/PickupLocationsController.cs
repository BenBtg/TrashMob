﻿namespace TrashMob.Controllers
{
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TrashMob.Models;
    using TrashMob.Shared.Managers.Interfaces;

    [Route("api/pickuplocations")]
    public class PickupLocationsController : KeyedController<PickupLocation>
    {
        private readonly IEventManager eventManager;

        public PickupLocationsController(IKeyedManager<PickupLocation> pickupLocationManager, IEventManager eventManager) 
            : base(pickupLocationManager)
        {
            this.eventManager = eventManager;
        }

        [HttpGet("{pickupLocationId}")]
        public async Task<IActionResult> Get(Guid pickupLocationId, CancellationToken cancellationToken)
        {
            return Ok(await Manager.GetAsync(pickupLocationId, cancellationToken).ConfigureAwait(false));
        }

        [HttpGet("getbyevent/{eventId}")]
        public async Task<IActionResult> GetByEvent(Guid eventId, CancellationToken cancellationToken)
        {
            return Ok(await Manager.GetByParentIdAsync(eventId, cancellationToken).ConfigureAwait(false));
        }

        [HttpPut]
        public async Task<IActionResult> Update(PickupLocation pickupLocation, CancellationToken cancellationToken)
        {
            // Todo: Add security
            var authResult = await AuthorizationService.AuthorizeAsync(User, pickupLocation, "UserOwnsEntity");

            if (!User.Identity.IsAuthenticated || !authResult.Succeeded)
            {
                return Forbid();
            }

            var result = await Manager.UpdateAsync(pickupLocation, UserId, cancellationToken).ConfigureAwait(false);
            TelemetryClient.TrackEvent(nameof(Update) + typeof(PickupLocation));

            return Ok(result);
        }

        [HttpPost]
        public override async Task<IActionResult> Add(PickupLocation instance, CancellationToken cancellationToken)
        {
            var mobEvent = eventManager.GetAsync(instance.EventId, cancellationToken);

            var authResult = await AuthorizationService.AuthorizeAsync(User, mobEvent, "UserOwnsEntity");

            if (!User.Identity.IsAuthenticated || !authResult.Succeeded)
            {
                return Forbid();
            }

            await Manager.AddAsync(instance, UserId, cancellationToken).ConfigureAwait(false);

            TelemetryClient.TrackEvent("AddPickupLocation");

            return Ok();
        }
    }
}
