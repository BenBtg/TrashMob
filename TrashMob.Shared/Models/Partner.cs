﻿#nullable disable

namespace TrashMob.Shared.Models
{
    using System.Collections.Generic;

    public partial class Partner : KeyedModel
    {
        public Partner()
        {
            PartnerContacts = new HashSet<PartnerContact>();
            PartnerDocuments = new HashSet<PartnerDocument>();
            PartnerNotes = new HashSet<PartnerNote>();
            PartnerSocialMediaAccounts = new HashSet<PartnerSocialMediaAccount>();
            PartnerLocations = new HashSet<PartnerLocation>();
            PartnerUsers = new HashSet<PartnerUser>();
        }

        public string Name { get; set; }

        public int PartnerStatusId { get; set; }

        public int PartnerTypeId { get; set; }

        public virtual ICollection<PartnerContact> PartnerContacts { get; set; }

        public virtual ICollection<PartnerDocument> PartnerDocuments { get; set; }

        public virtual ICollection<PartnerLocation> PartnerLocations { get; set; }

        public virtual ICollection<PartnerNote> PartnerNotes { get; set; }

        public virtual ICollection<PartnerSocialMediaAccount> PartnerSocialMediaAccounts { get; set; }

        public virtual ICollection<PartnerUser> PartnerUsers { get; set; }

        public virtual PartnerStatus PartnerStatus { get; set; }

        public virtual PartnerType PartnerType { get; set; }
    }
}
