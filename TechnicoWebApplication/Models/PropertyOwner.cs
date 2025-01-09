using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TechnicoWebApplication.Enums;

namespace TechnicoWebApplication.Models;
public class PropertyOwner : SoftDeletableEntity
{
    [Key]
    public required string Vat { get; set; }
    public string? Name { get; set; } 
    public string? Surname { get; set; } 
    public string? Address { get; set; } 
    public string? PhoneNumber { get; set; } 
    public required string Email { get; set; }
    public required string Password { get; set; }
    public UserType UserType { get; set; }
    public List<PropertyItem> PropertyItems { get; set; } = [];
}