using EMS.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Dtos
{
    public class LocalGetDto
    {
        public Guid Id { get; set; } //unikalny identyfikator
        public string Description { get; set; } = null!;
        public int LocalNumber { get; set; }
        public double Surface { get; set; }
        public bool NeedsRepair { get; set; }
        public ICollection<ReservationGetDto> ReservationsEntities { get; set; } = new List<ReservationGetDto>();
    }
}

