using CentristTraveler.Helper;
using CentristTraveler.Repositories.Implementations;
using CentristTraveler.Repositories.Interfaces;
using CentristTraveler.UnitOfWorks.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.UnitOfWorks.Implementations
{
    public class BaseUoW : IBaseUoW
    {
        #region Protected Members
        protected string _connectionString;
        protected IDbConnection _connection;
        protected IDbTransaction _transaction;

        #endregion

        #region Constructor
        public BaseUoW(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionString = connectionStrings.Value.DefaultConnection;
            _connection = new SqlConnection(_connectionString);
        }
        #endregion

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }

        }

    }
}
