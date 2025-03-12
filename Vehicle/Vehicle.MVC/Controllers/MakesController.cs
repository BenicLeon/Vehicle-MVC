using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vehicle.Service.Services;
using Vehicle.Common.ViewModels;
using Vehicle.Service.DTOs;
using AutoMapper;





namespace Vehicle.MVC.Controllers
{
    public class MakesController : Controller
    {
        private readonly IVehicleService _service;

        private readonly IMapper _mapper;

        private const int PageSize = 3;

        public MakesController(IVehicleService vehicleService, IMapper mapper)
        {
            _service = vehicleService;
            _mapper = mapper;
            
        }


        public async Task<IActionResult> Index(string searchString, string sortOrder, int pageNumber = 1)
        {
            var result = await _service.GetMakesAsync(searchString, sortOrder, pageNumber, PageSize);

            
            var viewModels = _mapper.Map<List<VehicleMakeViewModel>>(result.Items);

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["AbrvSortParam"] = sortOrder == "abrv" ? "abrv_desc" : "abrv";
            ViewData["SearchString"] = searchString;
            ViewData["CurrentPage"] = result.PageNumber;
            ViewData["TotalPages"] = result.TotalPages;

            return View(viewModels);
        }


        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleMakeViewModel makeViewModel)
        {
            
                if (ModelState.IsValid)
                {
                
                var makeDTO = _mapper.Map<VehicleMakeDTO>(makeViewModel);
                await _service.CreateMakeAsync(makeDTO);
                return RedirectToAction(nameof(Index));
                
                }
            
                return View(makeViewModel);
            
        }

        
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleMakeViewModel makeViewModel)
        {
            if (ModelState.IsValid)
            {
                
                var makeDto = _mapper.Map<VehicleMakeDTO>(makeViewModel);

                
                await _service.UpdateMakeAsync(makeDto);

                return RedirectToAction(nameof(Index)); 
            }

            return View(makeViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            
            var make = await _service.GetMakeByIdAsync(id);

            if (make == null)
            {
                return NotFound();
            }

            var makeViewModel = _mapper.Map<VehicleMakeViewModel>(make);
            return View(makeViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _service.DeleteMakeAsync(id);

            if (result)
            {
                
                return RedirectToAction("Index");
            }

            
            return NotFound();
        }


    }
}
