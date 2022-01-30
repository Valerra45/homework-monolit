﻿using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.Administration
{
    public class Employee
        : BaseEntity
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [MaxLength(50)]
        public string Email { get; set; }

        public virtual Role Role { get; set; }

        public int AppliedPromocodesCount { get; set; }
    }
}