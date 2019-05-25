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
        public List<Category> GetAll()
        {
            string sql = @"SELECT [Id]
                          ,[Name]
                          ,[CreatedBy]
                          ,[CreatedDate]
                          ,[UpdatedBy]
                          ,[UpdatedDate]
                      FROM [dbo].[Master_Category]";

            List<Category> categories = new List<Category>();

            try
            {
                categories = _connection.Query<Category>(sql, null, _transaction).ToList();
            }
            catch
            {
                categories = new List<Category>();
            }
            return categories;
        }
    }
}
