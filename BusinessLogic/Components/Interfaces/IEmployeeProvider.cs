using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public interface IEmployeeProvider
    {
        Int32 GetEmployeePageCount();
        Employee GetEmployee(Int32 id);
        GenericList<Employee> GetAllEmployee();
        GenericList<Employee> GetAllEmployee(Int32 pageNo, SortByEmployee sortBy, SortingOrder sortOrder);
        Employee InsertEmployee(Employee employee);
        Boolean UpdateEmployee(Employee employee);
        Boolean DeleteEmployee(Employee employee);
    }
}
