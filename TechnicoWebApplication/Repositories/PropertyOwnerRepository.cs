using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Context;
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
        PropertyOwner? propertyOwner = await _dbContext.PropertyOwners.FindAsync(id);

        if (propertyOwner == null)
        {
            return false;
        }

        _dbContext.PropertyOwners.Remove(propertyOwner);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<PropertyOwner?> Read(string id)
    {
        return await _dbContext.PropertyOwners.FindAsync(id);
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
}