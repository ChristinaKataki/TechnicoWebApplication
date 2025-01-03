using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Context;
using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Repositories;
public class PropertyOwnerRepository : IRepository<PropertyOwner, string>
{
    private readonly TechnicoDbContext _dbContext;

    public PropertyOwnerRepository(TechnicoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PropertyOwner> Create(PropertyOwner t)
    {
        _dbContext.PropertyOwners.Add(t);
        await _dbContext.SaveChangesAsync();
        return t;
    }

    public async Task<bool> Delete(string id)
    {
        PropertyOwner? propertyOwner = await _dbContext.PropertyOwners
        .Include(p => p.PropertyItems)
        .Include(p => p.Repairs)
        .FirstOrDefaultAsync(p => p.Vat == id);

        if (propertyOwner == null)
        {
            return false;
        }

        _dbContext.PropertyItems.RemoveRange(propertyOwner.PropertyItems);
        _dbContext.Repairs.RemoveRange(propertyOwner.Repairs);
        _dbContext.PropertyOwners.Remove(propertyOwner);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<PropertyOwner?> Read(string id)
    {
        return await _dbContext.PropertyOwners
            .Include(owner => owner.PropertyItems)
            .Include(owner => owner.Repairs)
            .FirstOrDefaultAsync(owner => owner.Vat == id);
    }

    public Task<List<PropertyOwner>> Read()
    {
        throw new NotImplementedException();
    }

    public async Task<PropertyOwner?> Update(string id, PropertyOwner propertyOwner)
    {
        var existingOwner = await _dbContext.PropertyOwners.FindAsync(id);

        if (existingOwner == null)
        {
            return null;
        }

        _dbContext.Entry(existingOwner).CurrentValues.SetValues(propertyOwner);

        await _dbContext.SaveChangesAsync();

        return existingOwner;
    }

    public async Task<PropertyOwner?> ReadByEmail(string email)
    {
        return await _dbContext.PropertyOwners
            .Where(owner => owner.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<IActionResult> ReadWithFilters(PropertyOwnerFilters filters)
    {
        var query = _dbContext.PropertyOwners.AsQueryable();

        if (!string.IsNullOrEmpty(filters.Email))
        {
            query = query.Where(owner => owner.Email.Contains(filters.Email));
        }

        if (!string.IsNullOrEmpty(filters.Vat))
        {
            query = query.Where(owner => owner.Vat == filters.Vat);
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