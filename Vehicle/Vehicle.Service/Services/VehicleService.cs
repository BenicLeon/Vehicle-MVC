using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vehicle.Common;
using Vehicle.Service.Data;
using Vehicle.Service.DTOs;
using Vehicle.Service.Models;

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

        #region Make CRUD

        public async Task<PagedResult<VehicleMakeDTO>> GetMakesAsync(string searchString = null, string sortOrder = "name", int pageNumber = 1, int pageSize = 3)
        {
            var query = _context.VehicleMakes.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(m => m.Name.Contains(searchString) || m.Abrv.Contains(searchString));
            }

            int totalCount = await query.CountAsync();
            query = ApplySorting(query, sortOrder);

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
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMakeAsync(VehicleMakeDTO modelDto)
        {
            var model = _mapper.Map<VehicleMake>(modelDto);
            _context.VehicleMakes.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task<VehicleMakeDTO> GetMakeByIdAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            return make == null ? null : _mapper.Map<VehicleMakeDTO>(make);
        }

        public async Task<bool> DeleteMakeAsync(int id)
        {
            return await DeleteEntityAsync(_context.VehicleMakes, id);
        }

        #endregion

        #region Model CRUD

        public async Task<PagedResult<VehicleModelDTO>> GetModelsAsync(int? makeId = null, string searchString = null, string sortOrder = "name", int pageNumber = 1, int pageSize = 3)
        {
            var query = _context.VehicleModels.Include(m => m.Make).AsQueryable();

            if (makeId.HasValue)
            {
                query = query.Where(m => m.MakeId == makeId.Value);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(m => m.Name.Contains(searchString) || m.Abrv.Contains(searchString));
            }

            int totalCount = await query.CountAsync();
            query = ApplySorting(query, sortOrder);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(m => _mapper.Map<VehicleModelDTO>(m))
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
            var model = await _context.VehicleModels.FindAsync(id);
            return model == null ? null : _mapper.Map<VehicleModelDTO>(model);
        }

        public async Task<bool> DeleteModelAsync(int id)
        {
            return await DeleteEntityAsync(_context.VehicleModels, id);
        }

        #endregion

        #region Helper Methods

        private IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortOrder) where T : class
        {
            sortOrder = sortOrder?.ToLower() ?? "name";
            return sortOrder switch
            {
                "name_desc" => query.OrderByDescending(m => EF.Property<string>(m, "Name")),
                "abrv" => query.OrderBy(m => EF.Property<string>(m, "Abrv")),
                "abrv_desc" => query.OrderByDescending(m => EF.Property<string>(m, "Abrv")),
                _ => query.OrderBy(m => EF.Property<string>(m, "Name")),
            };
        }

        private async Task<bool> DeleteEntityAsync<T>(DbSet<T> dbSet, int id) where T : class
        {
            try
            {
                var entity = await dbSet.FindAsync(id);
                if (entity == null)
                {
                    return false;
                }

                dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}