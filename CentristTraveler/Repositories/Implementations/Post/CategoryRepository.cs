using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CentristTraveler.Helper;
using CentristTraveler.Models;
using Microsoft.Extensions.Options;

using Dapper;
using CentristTraveler.Repositories.Interfaces;
using System.Data;

namespace CentristTraveler.Repositories.Implementations
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public async Task<IEnumerable<Category>> GetAll()
        {
            string sql = @"SELECT [Id] AS CategoryId
                          ,[Name]
                          ,[CreatedBy]
                          ,[CreatedDate]
                          ,[UpdatedBy]
                          ,[UpdatedDate]
                      FROM [dbo].[Master_Category]";

            try
            {
                return await _connection.QueryAsync<Category>(sql, null, _transaction);
            }
            catch (Exception ex)
            {
                return new List<Category>();
            }
        }
    }
}
