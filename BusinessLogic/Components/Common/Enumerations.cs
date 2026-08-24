using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interact.BusinessLogic
{
    public enum TransactionType
    {
        Select = 1,
        Insert = 2,
        Update = 3,
        Delete = 4
    }

    /// <summary>
    /// List of possible state for an entity.
    /// </summary>
    public enum EntityState
    {
        /// <summary>
        /// Entity is unchanged.
        /// </summary>
        Unchanged = 0,

        /// <summary>
        /// Entity is new.
        /// </summary>
        Added = 1,

        /// <summary>
        /// Entity has been changed.
        /// </summary>
        Changed = 2,

        /// <summary>
        /// Entity has been deleted.
        /// </summary>
        Deleted = 3
    }

    /// <summary>
    /// Enumeration of change actions.
    /// </summary>
    public enum ChangeType
    {
        Inserted,   // Added
        Updated,    // Replaced
        Deleted,    // Removed
        Truncated   // Cleared
    }

    #region Sorting
    public enum SortingOrder
    {
        Ascending = 0,
        Descending
    }

    public enum SortByZone
    {
        ID = 0,
        Name,
        Abbreviation
    }

    public enum SortByCustomer
    {
        CustomerNo = 0,
        LastName,
        FirstName
    }

    public enum SortByVehicle
    {
        Brand = 0,
        Model,
        Color
    }

    public enum SortBySales
    {
        SaleCode = 0,
        SaleDate,
        Customer,
        Vehicle
    }

    public enum SortByPayment
    {
        Sales = 0,
        PaymentNo,
        PaymentDate
    }

    public enum SortByEmployee
    {
        EmployeeNo = 0,
        LastName,
        FirstName
    }

    public enum SortByZoneGroup
    {
        ID = 0,
        Name
    }
    #endregion

    public enum Branch
    {
        HO_ = 1,
        Apalit_ = 2,
        Bagac_ = 3,
        Baliuag_ = 5,
        Bamban_ = 6,
        Bautista_ = 7,
        Calumpit_ = 8,
        Camiling_ = 9,
        Capas_ = 10,
        Concepcion_ = 11,
        Guagua_ = 12,
        Lapaz_ = 13,
        Lubao_ = 14,
        Magalang_ = 15,
        Malasiqui_ = 16,
        Mangaldan_ = 17,
        Norzagaray_ = 18,
        Orani_ = 19,
        Pandi_ = 20,
        Paniqui_ = 21,
        Porac_ = 22,
        Rosales_ = 23,
        SanFernando_ = 24,
        SanMiguel_ = 25,
        SanMiguel_ = 26,
        SantaAna_ = 27,
        SantaIgnacia_ =28,
        SantaMaria_ =29,
        SantaRita_ = 30,
        SJDM_ = 31,
        Audit_ = 253,
        Verifier_ = 254,
        Area_Supervisor_ = 255
    }

    public enum UserRole
    {
        Administrator = 0,
        Area_Supervisor,
        Account_Supervisor,
        Bookkeeper,
        Collector
    }

    public enum VehicleStatus
    {
        Brand_new = 0,
        Repossessed,
        Second_hand,
        Assume_balance,
        Sold = 255
        //All available = 512
    }

    public enum PaymentTerm
    {
        Cash = 0,
        Installment
    }

    public enum PaymentMode
    {
        Cash = 0,
        Check
    }

    public enum PaymentStatus
    {
        Processing = 0,
        Cleared,
        Cancelled
    }

    public enum SalesStatus
    {
        Current = 0,
        Repossess,
        History,
        Company_Service,
        Bad_Accounts,
        Brand_New_Cash_Sales,
        Inactive
    }

    public enum LedgerSource
    {
        Sales = 0,
        Payments,
        Notes,
        Penalties
    }

    public enum AmortStatus
    {
        Paid = 0,
        Not_paid
    }

    public enum ReportName
    {
        Collections = 0,
        Installment_Receivable,
        Payment_Frequency,
        Repossess_Balance,
        Sales_Summary,
        Ending_Report2
    }
}
