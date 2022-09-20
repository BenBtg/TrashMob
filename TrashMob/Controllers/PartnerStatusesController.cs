﻿
namespace TrashMob.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Mvc;
    using TrashMob.Shared.Persistence.Interfaces;

    [Route("api/partnerstatuses")]
    public class PartnerStatusesController : BaseController
    {
        private readonly IPartnerStatusRepository partnerStatusRepository;

        public PartnerStatusesController(TelemetryClient telemetryClient,
                                         IUserRepository userRepository,
                                         IPartnerStatusRepository partnerStatusRepository)
            : base(telemetryClient, userRepository)
        {
            this.partnerStatusRepository = partnerStatusRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPartnerStatuses(CancellationToken cancellationToken)
        {
            var result = await partnerStatusRepository.GetAllPartnerStatuses(cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }
    }
}
