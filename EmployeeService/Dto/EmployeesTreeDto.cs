using System.Collections.Generic;

namespace EmployeeService.Dto
{
    public  class EmployeesTreeDto
    {
        public EmployeeDto Employee { get; set; }
        public List<EmployeesTreeDto> Employees { get; set; } = new List<EmployeesTreeDto>();
    }
}