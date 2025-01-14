﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Dtos;
public class PropertyOwnerUpdateRequestDto
{
    public string? Name { get; set; } 
    public string? Surname { get; set; }
    public string? Address { get; set; } 
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}