using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicoWebApplication.Context;
using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Helpers;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Repositories;
public class RepairRepository : IRepository<Repair, long, RepairFilters>
{
    private readonly TechnicoDbContext _dbContext;

    public RepairRepository(TechnicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Repair> Create(Repair t)
    {
        _dbContext.Repairs.Add(t);
        await _dbContext.SaveChangesAsync();
        return t;
    }

    public async Task<bool> Delete(long id)
    {
        Repair? repair = _dbContext.Repairs.Find(id);

        if (repair == null)
        {
            return false;
        }

        _dbContext.Repairs.Remove(repair);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<Repair?> Read(long id)
    {
        return await _dbContext.Repairs
            .Include(repair => repair.PropertyItem.PropertyOwner)
            .FirstOrDefaultAsync(repair => repair.Id == id);
    }

    public async Task<List<Repair>> FindByOwner(string vat)
    {
        return await _dbContext.Repairs
            .Where(repair => repair.PropertyItem.PropertyOwner.Vat == vat)
            .ToListAsync();
    }

    public async Task<Repair?> Update(long id, Repair repair)
    {
        var existingRepair = await _dbContext.Repairs
            .Include(repair => repair.PropertyItem.PropertyOwner)
            .FirstOrDefaultAsync(repair => repair.Id == id);

        if (existingRepair == null)
        {
            return null;
        }

        _dbContext.Entry(existingRepair).CurrentValues.SetValues(repair);
        existingRepair.PropertyItem = repair.PropertyItem;

        await _dbContext.SaveChangesAsync();

        return existingRepair;
    }

    public async Task<PageResults<Repair>> ReadWithFilters(RepairFilters filters)
    {
        var query = _dbContext.Repairs.AsQueryable();

        if (!string.IsNullOrEmpty(filters.Vat))
        {
            query = query.Where(repair => repair.PropertyItem.PropertyOwner.Vat == filters.Vat);
        }

        if (filters.Status != null)
        {
            query = query.Where(repair => repair.Status == filters.Status);
        }

        if (filters.MinDate != null)
        {
            query = query.Where(repair => repair.RepairDate >= filters.MinDate);
        }

        if (filters.MaxDate != null)
        {
            query = query.Where(repair => repair.RepairDate <= filters.MaxDate);
        }

        var totalCount = await query.CountAsync();

        var elements = await query
            .Skip((filters.Page - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .Include(repair => repair.PropertyItem.PropertyOwner)
            .ToListAsync();

        return new PageResults<Repair>
        {
            TotalCount = totalCount,
            Page = filters.Page,
            PageSize = filters.PageSize,
            Elements = elements
        };
    }
}
