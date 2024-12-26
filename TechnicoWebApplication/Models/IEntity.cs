using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicoWebApplication.Models;
public interface IEntity<K>
{
    K Id { get; set; }
}