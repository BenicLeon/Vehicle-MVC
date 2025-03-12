using System.Threading.Tasks;
using Vehicle.Common;
using Vehicle.Service.DTOs;

namespace Vehicle.Service.Services
{
    public interface IVehicleService
    {
        #region Vehicle Make Operations

        Task<PagedResult<VehicleMakeDTO>> GetMakesAsync(
            string? searchString = null,
            string sortOrder = "name",
            int pageNumber = 1,
            int pageSize = 3);

        Task CreateMakeAsync(VehicleMakeDTO make);

        Task UpdateMakeAsync(VehicleMakeDTO make);

        Task<VehicleMakeDTO?> GetMakeByIdAsync(int id);

        Task<bool> DeleteMakeAsync(int id);

        #endregion

        #region Vehicle Model Operations

        Task<PagedResult<VehicleModelDTO>> GetModelsAsync(
            int? makeId = null,
            string? searchString = null,
            string sortOrder = "name",
            int pageNumber = 1,
            int pageSize = 3);

        Task CreateModelAsync(VehicleModelDTO model);

        Task UpdateModelAsync(VehicleModelDTO model);

        Task<VehicleModelDTO?> GetModelByIdAsync(int id);

        Task<bool> DeleteModelAsync(int id);
        #endregion
    }
}