﻿#nullable disable

namespace TrashMob.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class UserSubscription
    {
        public Guid UserId { get; set; }

        public Guid FollowingId { get; set; }

        public DateTimeOffset StartDate { get; set; }
        
        public DateTimeOffset? EndDate { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        
        public virtual UserProfile FollowingUser { get; set; }
    }
}
