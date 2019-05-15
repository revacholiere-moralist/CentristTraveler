using CentristTraveler.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Implementations
{
    public class BaseRepository : IBaseRepository
    {
        protected IDbTransaction _transaction;
        protected IDbConnection _connection;

        public void CreateTransaction(IDbTransaction transaction)
        {
            _transaction = transaction;
            _connection = transaction.Connection;
        }
    }
}
