using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle.Service.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }

        [Required]
        public int MakeId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = String.Empty;

        [StringLength(20)]
        public string Abrv { get; set; } = String.Empty;

        public VehicleMake? Make { get; set; }


    }
}
