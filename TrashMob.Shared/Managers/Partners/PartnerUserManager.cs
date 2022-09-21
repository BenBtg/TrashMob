﻿namespace TrashMob.Shared.Managers.Partners
{
    using TrashMob.Shared.Managers.Interfaces;
    using TrashMob.Shared.Models;
    using TrashMob.Shared.Persistence.Interfaces;

    public class PartnerUserManager : BaseManager<PartnerUser>, IBaseManager<PartnerUser>
    {
        public PartnerUserManager(IBaseRepository<PartnerUser> partnerUserRepository) : base(partnerUserRepository)
        {
        }
    }
}
