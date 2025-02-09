using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Entities
{
    public class LocalEntity
    {
        public Guid Id { get; set; } //unikalny identyfikator
        public string Description { get; set; } = null!;
        public int LocalNumber { get; set; } 
        public double Surface { get; set; } 
        public bool NeedsRepair { get; set; } 
        public string AppUserId { get; set; } = null!;
        public AppUserEntity AppUserEntity { get; set; } = null!;
        public ICollection<ReservationEntity> ReservationsEntities { get; set; } = new List<ReservationEntity>();

    }
}
