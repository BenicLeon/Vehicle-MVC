using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vehicle.Common;
using Vehicle.Common.ViewModels;
using Vehicle.Service.DTOs;
using Vehicle.Service.Services;

namespace Vehicle.MVC.Controllers
{
    public class ModelController(IVehicleService vehicleService, IMapper mapper) : Controller
    {
        private readonly IVehicleService _service = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private const int PageSize = 3;

        #region Model CRUD

        public async Task<IActionResult> Index(int? makeId, string? searchString, string? sortOrder, int pageNumber = 1)
        {
            try
            {
                var modelsResult = await _service.GetModelsAsync(makeId, searchString, sortOrder ?? "name", pageNumber, PageSize);
                var makesResult = await _service.GetMakesAsync();

                var viewModels = modelsResult.Items
                    .Select(m => new VehicleModelViewModel
                    {
                        Id = m.Id,
                        MakeId = m.MakeId,
                        Name = m.Name,
                        Abrv = m.Abrv,
                        MakeName = makesResult.Items.FirstOrDefault(make => make.Id == m.MakeId)?.Name ?? "Unknown"
                    })
                    .ToList();

                SetViewData(modelsResult, sortOrder ?? "name", searchString, makeId);
                ViewData["Makes"] = new SelectList(makesResult.Items, "Id", "Name");

                return View(viewModels);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving models.");
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                await PopulateMakesDropdownAsync();
                return View(new VehicleModelViewModel());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while loading the create page.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleModelViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateMakesDropdownAsync(viewModel.MakeId);
                return View(viewModel);
            }

            try
            {
                var modelDto = _mapper.Map<VehicleModelDTO>(viewModel);
                await _service.CreateModelAsync(modelDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                await PopulateMakesDropdownAsync(viewModel.MakeId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the model.");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var modelDto = await _service.GetModelByIdAsync(id);
                if (modelDto == null)
                {
                    return NotFound();
                }

                var viewModel = _mapper.Map<VehicleModelViewModel>(modelDto);
                await PopulateMakesDropdownAsync(viewModel.MakeId);
                return View(viewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the model for editing.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleModelViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await PopulateMakesDropdownAsync(viewModel.MakeId);
                return View(viewModel);
            }

            try
            {
                var modelDto = _mapper.Map<VehicleModelDTO>(viewModel);
                await _service.UpdateModelAsync(modelDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                await PopulateMakesDropdownAsync(viewModel.MakeId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the model.");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var modelDto = await _service.GetModelByIdAsync(id);
                if (modelDto == null)
                {
                    return NotFound();
                }

                var viewModel = _mapper.Map<VehicleModelViewModel>(modelDto);
                return View(viewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the model for deletion.");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _service.DeleteModelAsync(id);
                return success ? RedirectToAction(nameof(Index)) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the model.");
            }
        }

        #endregion

        #region Private Helpers

        private void SetViewData(PagedResult<VehicleModelDTO> result, string sortOrder, string? searchString, int? makeId)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["AbrvSortParam"] = sortOrder == "abrv" ? "abrv_desc" : "abrv";
            ViewData["SearchString"] = searchString;
            ViewData["CurrentPage"] = result.PageNumber;
            ViewData["TotalPages"] = result.TotalPages;
            ViewData["MakeId"] = makeId;
        }

        private async Task PopulateMakesDropdownAsync(int? selectedMakeId = null)
        {
            var makes = await _service.GetMakesAsync();
            ViewBag.Makes = new SelectList(makes.Items, "Id", "Name", selectedMakeId);
        }

        #endregion
    }
}