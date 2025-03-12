using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vehicle.Common;
using Vehicle.Common.ViewModels;
using Vehicle.Service.DTOs;
using Vehicle.Service.Services;

namespace Vehicle.MVC.Controllers
{
    public class MakeController : Controller
    {
        private readonly IVehicleService _service;
        private readonly IMapper _mapper;
        private const int PageSize = 3;

        public MakeController(IVehicleService vehicleService, IMapper mapper)
        {
            _service = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region Make CRUD

        public async Task<IActionResult> Index(string? searchString, string? sortOrder, int pageNumber = 1)
        {
            try
            {
                var result = await _service.GetMakesAsync(searchString, sortOrder, pageNumber, PageSize);
                var viewModels = _mapper.Map<List<VehicleMakeViewModel>>(result.Items);

                SetViewData(result, sortOrder, searchString);
                return View(viewModels);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving makes.");
            }
        }

        public IActionResult Create()
        {
            return View(new VehicleMakeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleMakeViewModel makeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(makeViewModel);
            }

            try
            {
                var makeDto = _mapper.Map<VehicleMakeDTO>(makeViewModel);
                await _service.CreateMakeAsync(makeDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the make.");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var makeDto = await _service.GetMakeByIdAsync(id);
                if (makeDto == null)
                {
                    return NotFound();
                }

                var viewModel = _mapper.Map<VehicleMakeViewModel>(makeDto);
                return View(viewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the make for editing.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleMakeViewModel makeViewModel)
        {
            if (id != makeViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(makeViewModel);
            }

            try
            {
                var makeDto = _mapper.Map<VehicleMakeDTO>(makeViewModel);
                await _service.UpdateMakeAsync(makeDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the make.");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var makeDto = await _service.GetMakeByIdAsync(id);
                if (makeDto == null)
                {
                    return NotFound();
                }

                var viewModel = _mapper.Map<VehicleMakeViewModel>(makeDto);
                return View(viewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the make for deletion.");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _service.DeleteMakeAsync(id);
                return success ? RedirectToAction(nameof(Index)) : NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the make.");
            }
        }

        #endregion

        #region Private Helpers

        private void SetViewData(PagedResult<VehicleMakeDTO> result, string? sortOrder, string? searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["AbrvSortParam"] = sortOrder == "abrv" ? "abrv_desc" : "abrv";
            ViewData["SearchString"] = searchString;
            ViewData["CurrentPage"] = result.PageNumber;
            ViewData["TotalPages"] = result.TotalPages;
        }

        #endregion
    }
}