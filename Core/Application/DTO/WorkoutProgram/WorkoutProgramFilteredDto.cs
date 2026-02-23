using Application.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.WorkoutProgram
{
    public class WorkoutProgramFilteredDto : PagedRequest
    {
        public bool IsSystemProgram { get; set; } 
        public bool IsUserProgram { get; set; }  
        public string? Level { get; set; } 
        public List<string>? Ambition { get; set; } 
        public string? SearchText { get; set; }  
        
    }
}
