using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle.Service.Models
{
    public class VehicleMake
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string? Name { get; set; }

        [StringLength(20)]
        public string? Abrv {  get; set; }

        public virtual List<VehicleModel> Vehicles { get; set; } = new List<VehicleModel>();


    }
}
