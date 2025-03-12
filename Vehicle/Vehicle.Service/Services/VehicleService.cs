using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle.Service.Data;
using Vehicle.Service.Models;
using Vehicle.Service.DTOs;
using Vehicle.Common;


namespace Vehicle.Service.Services
{
    public class VehicleService : IVehicleService
    {

        private readonly VehicleDbContext _context;
        private readonly IMapper _mapper;

        public VehicleService(VehicleDbContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }
        #region Make crud
        public async Task<PagedResult<VehicleMakeDTO>> GetMakesAsync(string searchString = null,
            string sortOrder = "name", int pageNumber = 1, int pageSize = 3)
        {
            sortOrder = sortOrder?.ToLower() ?? "name";

            var query = _context.VehicleMakes.AsQueryable();

            
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(m => m.Name.Contains(searchString) ||
                                      m.Abrv.Contains(searchString));
            }

            
            int totalCount = await query.CountAsync();

            
            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(m => m.Name),
                "abrv" => query.OrderBy(m => m.Abrv),
                "abrv_desc" => query.OrderByDescending(m => m.Abrv),
                _ => query.OrderBy(m => m.Name),
            };

            
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(m => _mapper.Map<VehicleMakeDTO>(m))
                .ToListAsync();

            return new PagedResult<VehicleMakeDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task CreateMakeAsync(VehicleMakeDTO modelDto)
        {
            var model = _mapper.Map<VehicleMake>(modelDto);
            await _context.VehicleMakes.AddAsync(model);
            _context.SaveChanges();
            
        }

        public async Task UpdateMakeAsync(VehicleMakeDTO modelDto)
        {
            var model = _mapper.Map<VehicleMake>(modelDto);
            _context.VehicleMakes.Update(model);
            await _context.SaveChangesAsync();
             

        }
        public async Task<VehicleMakeDTO> GetMakeByIdAsync(int id)
        {
            var make = await _context.VehicleMakes
                                      .Where(m => m.Id == id)
                                      .FirstOrDefaultAsync(); 

            if (make == null)
            {
                return null; 
            }

            
            return _mapper.Map<VehicleMakeDTO>(make);
        }

        public async Task<bool> DeleteMakeAsync(int id)
        {
            try
            {
                var make = await _context.VehicleMakes.FindAsync(id);
                if (make == null )
                {
                    return false;
                }

                _context.VehicleMakes.Remove(make);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }


        }
        #endregion
        #region Model Crud

        public async Task<PagedResult<VehicleModelDTO>> GetModelsAsync(int? makeId = null, string searchString = null, string sortOrder = "name", int pageNumber = 1, int pageSize = 3)
        {
            sortOrder = sortOrder?.ToLower() ?? "name";

            var query = _context.VehicleModels
                .Include(m => m.Make) 
                .AsQueryable();

            if (makeId.HasValue)
            {
                query = query.Where(m => m.MakeId == makeId.Value);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(m => m.Name.Contains(searchString) || m.Abrv.Contains(searchString));
            }

            int totalCount = await query.CountAsync();

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(m => m.Name),
                "abrv" => query.OrderBy(m => m.Abrv),
                "abrv_desc" => query.OrderByDescending(m => m.Abrv),
                _ => query.OrderBy(m => m.Name),
            };

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new VehicleModelDTO
                {
                    Id = m.Id,
                    MakeId = m.MakeId,
                    Name = m.Name,
                    Abrv = m.Abrv
                })
                .ToListAsync();

            return new PagedResult<VehicleModelDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task CreateModelAsync(VehicleModelDTO modelDto)
        {
            var model = _mapper.Map<VehicleModel>(modelDto);
            await _context.VehicleModels.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateModelAsync(VehicleModelDTO modelDto)
        {
            var model = _mapper.Map<VehicleModel>(modelDto);
            _context.VehicleModels.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task<VehicleModelDTO> GetModelByIdAsync(int id)
        {
            var model = await _context.VehicleModels
                                      .Where(m => m.Id == id)
                                      .FirstOrDefaultAsync();

            if (model == null)
            {
                return null;
            }

            return _mapper.Map<VehicleModelDTO>(model);
        }

        public async Task<bool> DeleteModelAsync(int id)
        {
            try
            {
                var model = await _context.VehicleModels.FindAsync(id);
                if (model == null)
                {
                    return false;
                }

                _context.VehicleModels.Remove(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion



    }
}

