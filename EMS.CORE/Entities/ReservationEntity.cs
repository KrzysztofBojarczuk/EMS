using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Entities
{
    public class ReservationEntity
    {
        public Guid Id { get; set; }
        public Guid LocalId { get; set; }
        public LocalEntity LocalEntity { get; set; }
        public string? AppUserId { get; set; } = null!;
        public AppUserEntity AppUserEntity { get; set; } = null!;
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
    }
}
