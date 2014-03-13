using System.Data;
using NHibernate;
using System;

namespace NHibernateTest.General
{
    /// <summary>
    /// Patterns Unit of Work 
    /// Encapsulates the transaction functionalities by exposing commit and rollback operations
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ITransaction transaction;
        public ISession Session { get; private set; }

        /// <summary>
        /// Initialize all variables and open session
        /// </summary>
        /// <param name="sessionFactory">Session of nhibernate with database</param>
        public UnitOfWork(ISessionFactory sessionFactory)
        {
            Session = sessionFactory.OpenSession();
            Session.FlushMode = FlushMode.Auto;
            transaction = Session.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// Commit to current transaction
        /// </summary>
        public void Commit()
        {
            if (!transaction.IsActive)
            {
                throw new InvalidOperationException("Not active transaction");
            }
            transaction.Commit();
        }

        /// <summary>
        /// Rollback to current transaction 
        /// </summary>
        public void Rollback()
        {
            if (transaction.IsActive)
            {
                transaction.Rollback();
            }
        }

        /// <summary>
        /// Close session
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method. 
            // Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue  
            // and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }
    }

    /// <summary>
    /// Encapsulates the transaction functionalities by exposing commit and rollback operations.
    /// Implement IDisposable
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
