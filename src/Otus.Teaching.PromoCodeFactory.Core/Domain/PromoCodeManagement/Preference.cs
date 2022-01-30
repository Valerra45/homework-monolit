using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Preference
        : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual List<Customer> Customers { get; set; } = new List<Customer>();
    }
}