using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Helpers;

namespace TechnicoWebApplication.Repositories;
public interface IRepository<T, K, F>
{
    Task<T> Create(T t);
    Task<T?> Read(K id);
    Task<T?> Update(K id, T t);
    Task<bool> Delete(K id);
    Task<PageResults<T>> ReadWithFilters(F filters);
}
