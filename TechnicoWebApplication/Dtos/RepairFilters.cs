using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Enums;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Dtos;
public class RepairFilters
{
    public string? Vat { get; set; }
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate {  get; set; }
    public RepairStatus? Status { get; set; }
    public int Page { get; set; } = 1; 
    public int PageSize { get; set; } = 10;
}
