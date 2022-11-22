﻿namespace TrashMob.Shared.Managers.Partners
{
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using TrashMob.Models;
    using TrashMob.Shared.Managers.Interfaces;
    using TrashMob.Shared.Persistence.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using TrashMob.Shared.Poco;

    public class PartnerAdminInvitationManager : KeyedManager<PartnerAdminInvitation>, IPartnerAdminInvitationManager
    {
        private readonly IBaseManager<PartnerAdmin> partnerAdminManager;
        private readonly IUserManager userManager;

        public PartnerAdminInvitationManager(IKeyedRepository<PartnerAdminInvitation> partnerAdminInvitationRepository,
                                             IBaseManager<PartnerAdmin> partnerAdminManager,
                                             IUserManager userManager)
            : base(partnerAdminInvitationRepository)
        {
            this.partnerAdminManager = partnerAdminManager;
            this.userManager = userManager;
        }

        public override async Task<IEnumerable<PartnerAdminInvitation>> GetByParentIdAsync(Guid parentId, CancellationToken cancellationToken)
        {
            return (await Repository.Get().Where(p => p.PartnerId == parentId).ToListAsync(cancellationToken)).AsEnumerable();
        }

        public async Task<Partner> GetPartnerForInvitation(Guid partnerAdminInvitationId, CancellationToken cancellationToken)
        {
            var partnerInvitation = await Repository.Get(p => p.Id == partnerAdminInvitationId, false)
                                        .Include(p => p.Partner)
                                        .FirstOrDefaultAsync(cancellationToken);
            return partnerInvitation.Partner;
        }

        public override async Task<PartnerAdminInvitation> AddAsync(PartnerAdminInvitation instance, Guid userId, CancellationToken cancellationToken = default)
        {
            // Check to see if this user already exists in the system.
            var existingUser = await userManager.GetUserByEmailAsync(instance.Email, cancellationToken);
            var newInvitation = await base.AddAsync(instance, userId, cancellationToken);

            if (existingUser == null)
            {
                // Todo - Send Email to invite to join trashmob and join partner
            }
            else
            {
                // Todo - Send Email to invite user to join partner
            }

            return newInvitation;
        }

        public async Task AcceptInvitation(Guid partnerAdminInviationId, Guid userId, CancellationToken cancellationToken)
        {
            var partnerAdminInvitation = await Repository.Get(pa => pa.Id == partnerAdminInviationId).FirstOrDefaultAsync(cancellationToken);

            if (partnerAdminInvitation == null)
            {
                return;
            }

            var partnerAdmin = new PartnerAdmin
            {
                PartnerId = partnerAdminInvitation.PartnerId,
                UserId = userId
            };

            await partnerAdminManager.AddAsync(partnerAdmin, cancellationToken);
            partnerAdminInvitation.InvitationStatusId = (int)InvitationStatusEnum.Accepted;
            await base.UpdateAsync(partnerAdminInvitation, userId, cancellationToken);
        }

        public async Task DeclineInvitation(Guid partnerAdminInviationId, Guid userId, CancellationToken cancellationToken)
        {
            var partnerAdminInvitation = await Repository.Get(pa => pa.Id == partnerAdminInviationId).FirstOrDefaultAsync(cancellationToken);

            if (partnerAdminInvitation == null)
            {
                return;
            }

            partnerAdminInvitation.InvitationStatusId = (int)InvitationStatusEnum.Declined;
            await base.UpdateAsync(partnerAdminInvitation, userId, cancellationToken);
        }

        public async Task<PartnerAdminInvitation> ResendPartnerAdminInvitation(Guid partnerAdminInvitationId, Guid UserId, CancellationToken cancellationToken)
        {
            // Check to see if this user already exists in the system.
            var instance = await Repo.GetAsync(partnerAdminInvitationId, cancellationToken);
            var existingUser = await userManager.GetUserByEmailAsync(instance.Email, cancellationToken);

            if (existingUser == null)
            {
                // Todo - Send Email to invite to join trashmob and join partner
            }
            else
            {
                // Todo - Send Email to invite user to join partner
            }

            instance.InvitationStatusId = (int)InvitationStatusEnum.Sent;
            var newInvitation = await base.UpdateAsync(instance, UserId, cancellationToken);
            return newInvitation;
        }

        public async Task<IEnumerable<DisplayPartnerAdminInvitation>> GetInvitationsForUser(Guid userId, CancellationToken cancellationToken)
        {
            var user = await userManager.GetAsync(userId, cancellationToken);

            var partnerInvitations = await Repository.Get(p => p.Email == user.Email)
                                                     .Include(p => p.Partner)
                                                     .ToListAsync(cancellationToken);

            var displayInvitations = new List<DisplayPartnerAdminInvitation>();
            foreach (var invitation in partnerInvitations)
            {
                var displayInvitation = new DisplayPartnerAdminInvitation
                {
                    Id = invitation.Id,
                    PartnerName = invitation.Partner.Name
                };

                displayInvitations.Add(displayInvitation);
            }

            return displayInvitations;
        }
    }
}