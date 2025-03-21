﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle.Common.ViewModels
{
    public class VehicleModelViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please select a Make")]
        public int? MakeId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Abbreviation is required")]
        public string Abrv { get; set; }

        public string? MakeName {  get; set; }
    }
}
