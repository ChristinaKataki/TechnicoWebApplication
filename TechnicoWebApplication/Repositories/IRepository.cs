using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicoWebApplication.Repositories;
public interface IRepository<T, K>
{
    Task<T> Create(T t);
    Task<T?> Read(K id);
    Task<List<T>> Read();
    Task<T?> Update(K id, T t);
    Task<bool> Delete(K id);
}
