using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicoWebApplication.Context;
using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Repositories;

public class PropertyItemRepository : IRepository<PropertyItem, string>
{
    private readonly TechnicoDbContext _dbContext;

    public PropertyItemRepository(TechnicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PropertyItem> Create(PropertyItem t)
    {
        _dbContext.PropertyItems.Add(t);
        await _dbContext.SaveChangesAsync();
        return t;
    }

    public async Task<bool> Delete(string id)
    {
        PropertyItem? propertyItem = _dbContext.PropertyItems.Find(id);

        if (propertyItem == null)
        {
            return false;
        }

        _dbContext.PropertyItems.Remove(propertyItem);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<PropertyItem?> Read(string id)
    {
        return await _dbContext.PropertyItems
            .Include(item => item.PropertyOwner)
            .FirstOrDefaultAsync(item => item.Id == id);
    }

    public async Task<List<PropertyItem>> FindByOwner(string vat)
    {
        return await _dbContext.PropertyItems
            .Where(item => item.PropertyOwner.Vat == vat)
            .ToListAsync();
    }

    public Task<List<PropertyItem>> Read()
    {
        throw new NotImplementedException();
    }

    public async Task<PropertyItem?> Update(string id, PropertyItem propertyItem)
    {
        var existingItem = await _dbContext.PropertyItems.FindAsync(id);

        if (existingItem == null)
        {
            return null;
        }

        _dbContext.Entry(existingItem).CurrentValues.SetValues(propertyItem);

        await _dbContext.SaveChangesAsync();

        return existingItem;
    }

    public async Task<IActionResult> ReadWithFilters(PropertyItemFilters filters)
    {
        var query = _dbContext.PropertyItems.AsQueryable();

        if (!string.IsNullOrEmpty(filters.Id))
        {
            query = query.Where(item => item.Id == filters.Id);
        }

        if (!string.IsNullOrEmpty(filters.Vat))
        {
            query = query.Where(item => item.PropertyOwner.Vat == filters.Vat);
        }

        var totalCount = await query.CountAsync();

        var elements = await query
            .Skip((filters.Page - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .ToListAsync();

        return new OkObjectResult(new
        {
            TotalCount = totalCount,
            Page = filters.Page,
            PageSize = filters.PageSize,
            Elements = elements
        });
    }
}