using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicoWebApplication.Models;
public abstract class SoftDeletableEntity
{
    public bool IsDeleted { get; set; } = false;
}