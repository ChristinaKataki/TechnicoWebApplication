﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Dtos;
public class PropertyItemFilters
{
    public string? Id { get; set; }
    public string? Vat { get; set; }
    public int Page { get; set; } = 1; 
    public int PageSize { get; set; } = 10;
}
