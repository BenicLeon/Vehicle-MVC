using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vehicle.Service.Services;
using Vehicle.Common.ViewModels;
using Vehicle.Service.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;





namespace Vehicle.MVC.Controllers
{
    public class ModelController : Controller
    {
        private readonly IVehicleService _service;

        private readonly IMapper _mapper;

        private const int PageSize = 3;

        public ModelController(IVehicleService vehicleService, IMapper mapper)
        {
            _service = vehicleService;
            _mapper = mapper;

        }

        #region Model Crud
        public async Task<IActionResult> Index(int? makeId, string searchString, string sortOrder = "name", int pageNumber = 1)
        {
            var modelsResult = await _service.GetModelsAsync(makeId, searchString, sortOrder, pageNumber, PageSize);
            var makesResult = await _service.GetMakesAsync();

            var viewModels = modelsResult.Items.Select(m => new VehicleModelViewModel
            {
                Id = m.Id,
                MakeId = m.MakeId,
                Name = m.Name,
                Abrv = m.Abrv,
                MakeName = makesResult.Items.FirstOrDefault(make => make.Id == m.MakeId)?.Name ?? "Unknown"

            }).ToList();
               
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["AbrvSortParam"] = sortOrder == "abrv" ? "abrv_desc" : "abrv";
            ViewData["SearchString"] = searchString;
            ViewData["CurrentPage"] = modelsResult.PageNumber;
            ViewData["TotalPages"] = modelsResult.TotalPages;
            ViewData["MakeId"] = makeId;

            
            ViewData["Makes"] = new SelectList(makesResult.Items, "Id", "Name");

            return View(viewModels);
        }



        public async Task<IActionResult> Create()
        {
            var makes = await _service.GetMakesAsync();
            ViewBag.Makes = new SelectList(makes.Items, "Id", "Name");
            return View();
        }

        // POST: Model/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleModelViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var modelDto = _mapper.Map<VehicleModelDTO>(viewModel);
                await _service.CreateModelAsync(modelDto);
                return RedirectToAction(nameof(Index));
            }

            var makes = await _service.GetMakesAsync();
            ViewBag.Makes = new SelectList(makes.Items, "Id", "Name", viewModel.MakeId);
            return View(viewModel);
        }

        // GET: Model/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var modelDto = await _service.GetModelByIdAsync(id);
            if (modelDto == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<VehicleModelViewModel>(modelDto);
            var makes = await _service.GetMakesAsync();
            ViewBag.Makes = new SelectList(makes.Items, "Id", "Name", viewModel.MakeId);
            return View(viewModel);
        }

        // POST: Model/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleModelViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var modelDto = _mapper.Map<VehicleModelDTO>(viewModel);
                await _service.UpdateModelAsync(modelDto);
                return RedirectToAction(nameof(Index));
            }

            var makes = await _service.GetMakesAsync();
            ViewBag.Makes = new SelectList(makes.Items, "Id", "Name", viewModel.MakeId);
            return View(viewModel);
        }

        // GET: Model/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var modelDto = await _service.GetModelByIdAsync(id);
            if (modelDto == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<VehicleModelViewModel>(modelDto);
            return View(viewModel);
        }

        // POST: Model/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteModelAsync(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
