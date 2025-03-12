using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle.Service.Models;
using Vehicle.Service.DTOs;
using Vehicle.Common;


namespace Vehicle.Service.Services
{
    public interface IVehicleService
    {
        Task<PagedResult<VehicleMakeDTO>> GetMakesAsync(string searchString = null,
            string sortOrder = "name", int pageNumber = 1, int pageSize = 3);
        
        Task CreateMakeAsync(VehicleMakeDTO make);
        Task UpdateMakeAsync(VehicleMakeDTO make);
        Task<VehicleMakeDTO> GetMakeByIdAsync(int id);
        Task<bool> DeleteMakeAsync(int id);

        Task<PagedResult<VehicleModelDTO>> GetModelsAsync(int? makeId,string searchString = null,
            string sortOrder = "name", int pageNumber = 1, int pageSize = 3);

        Task CreateModelAsync(VehicleModelDTO modelDto);
        Task UpdateModelAsync(VehicleModelDTO modelDto);
        Task<VehicleModelDTO> GetModelByIdAsync(int id);
        Task<bool> DeleteModelAsync(int id);
    }
}
