using System;
using System.ComponentModel;

namespace Interact.BusinessLogic
{
    /// <summary>
    /// The interface that each business object implements.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Returns the entity's status.
        /// </summary>
        EntityState EntityState
        {
            get;
        }

        /// <summary>
        ///	The name of the entity's underlying database table.
        /// </summary>
        string EntityTable
        {
            get;
        }

        /// <summary>
        /// Accepts the changes made to the entity.
        /// </summary>
        void AcceptChanges();

        /// <summary>
        /// Marks entity to be deleted.
        /// </summary>
        void MarkToDelete();
    }

    public interface IPerson : IEntity
    {
        string ID
        {
            get;
            set;
        }

        string Lastname
        {
            get;
            set;
        }

        string Firstname
        {
            get;
            set;
        }

        string Middlename
        {
            get;
            set;
        }

        string Nickname
        {
            get;
            set;
        }
    }

    public interface ITable
    {
    }
}
