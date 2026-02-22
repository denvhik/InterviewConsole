using EmployeeService.Data;
using EmployeeService.Dto;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EmployeeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeRepository _repository = new EmployeeRepository();
        public EmployeesTreeDto GetEmployeeById(int id)
        {
            if (id <= 0) return null;
         
            List<EmployeeDto> employee = _repository.GetEmployeesById(id);
            if (employee == null || employee.Count == 0) return null;

            return EmployeesTree(employee, id);
        }

        public void EnableEmployee(int id, bool enabled)
        {
            if (id <= 0) return;

            _repository.SetEmployeeEnabled(id, enabled);
        }

        private EmployeesTreeDto EmployeesTree(List<EmployeeDto> employee, int Id)
        {
            if (employee == null || employee.Count == 0)
                return null;

            Dictionary<int, EmployeeDto> employeeById =
                employee.ToDictionary(e => e.Id);

            Dictionary<int, List<int>> EmployeesByManagerId =
                employee
                    .Where(e => e.ManagerId.HasValue)
                    .GroupBy(e => e.ManagerId.Value)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.Id).ToList()
                    );

            EmployeesTreeDto BuildNode(int currentEmployeeId)
            {
                var node = new EmployeesTreeDto
                {
                    Employee = employeeById[currentEmployeeId]
                };

                if (EmployeesByManagerId.TryGetValue(currentEmployeeId, out var directReportIds))
                {
                    foreach (var childEmployeeId in directReportIds.OrderBy(x => x))
                    {
                        node.Employees.Add(BuildNode(childEmployeeId));
                    }
                }

                return node;
            }

            return employeeById.ContainsKey(Id)
                ? BuildNode(Id)
                : null;
        }

    }

      
}