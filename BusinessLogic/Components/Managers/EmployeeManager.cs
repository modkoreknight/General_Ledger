using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public class EmployeeManager
    {
        #region Fields
        private IEmployeeProvider _provider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public EmployeeManager(IEmployeeProvider provider)
        {
            this._provider = provider;
        }
        #endregion

        #region Methods
        public Int32 GetEmployeePageCount()
        {
            return this._provider.GetEmployeePageCount();
        }

        public Employee GetEmployee(Int32 id)
        {
            return this._provider.GetEmployee(id);
        }

        public GenericList<Employee> GetAllEmployee()
        {
            return this._provider.GetAllEmployee();
        }

        public GenericList<Employee> GetAllEmployee(Int32 pageNo, SortByEmployee sortBy, SortingOrder sortOrder)
        {
            return this._provider.GetAllEmployee(pageNo, sortBy, sortOrder);
        }

        public Employee InsertEmployee(Employee employee)
        {
            return this._provider.InsertEmployee(employee);
        }

        public Boolean UpdateEmployee(Employee employee)
        {
            return this._provider.UpdateEmployee(employee);
        }

        public Boolean DeleteEmployee(Employee employee)
        {
            return this._provider.DeleteEmployee(employee);
        }
        #endregion
    }
}
