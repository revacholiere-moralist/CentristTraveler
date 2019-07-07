using CentristTraveler.Models;
using CentristTraveler.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Implementations
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            string sql = @"SELECT [Id]
                          ,[Name]
                          ,[CreatedBy]
                          ,[CreatedDate]
                          ,[UpdatedBy]
                          ,[UpdatedDate]
                      FROM [dbo].[Role]";
            return await _connection.QueryAsync<Role>(sql, null, _transaction);
        }

        public async Task<Role> GetRoleById(int id)
        {
            string sql = @"SELECT [Id]
                          ,[Name]
                          ,[CreatedBy]
                          ,[CreatedDate]
                          ,[UpdatedBy]
                          ,[UpdatedDate]
                      FROM [dbo].[Role]
                      WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Role>(sql,
                new
                {
                    @Id = id
                },
                _transaction);
        }

        public async Task<int> Create(Role role)
        {
            string sql = @"INSERT INTO [dbo].[Role]
                           ([Name]
                           ,[CreatedBy]
                           ,[CreatedDate]
                           ,[UpdatedBy]
                           ,[UpdatedDate])
                     VALUES
                           (@Name
                           ,@CreatedBy
                           ,@CreatedDate
                           ,@UpdatedBy
                           ,@UpdatedDate
                        ,SELECT CAST(SCOPE_IDENTITY() as int";

            return await _connection.ExecuteScalarAsync<int>(sql,
                new
                {
                    @Name = role.Name,
                    @CreatedBy = role.CreatedBy,
                    @CreatedDate = role.CreatedDate,
                    @UpdatedBy = role.UpdatedBy,
                    @UpdatedDate = role.UpdatedDate
                },
               _transaction);
        }

        public async Task<bool> Update(Role role)
        {
            string sql = @"UPDATE [dbo].[Role]
                           SET [Name] = @Name
                              ,[CreatedBy] = @CreatedBy
                              ,[CreatedDate] = @CreatedDate
                              ,[UpdatedBy] = @UpdatedBy
                              ,[UpdatedDate] = @UpdatedDate
                         WHERE Id = @Id";
            bool isSuccess = false;

            int affectedRows = await _connection.ExecuteAsync(sql,
                new
                {
                    @Id = role.RoleId,
                    @Name = role.Name,
                    @UpdatedBy = role.UpdatedBy,
                    @UpdatedDate = role.UpdatedDate
                },
               _transaction);
            if (affectedRows > 0)
            {
                isSuccess = true;
            }

            return isSuccess;
        }
        public async Task<bool> Delete(int id)
        {
            string sql = @"DELETE FROM [dbo].[Role]
                WHERE Id = @Id";
            bool isSuccess = false;

            int affectedRows = await _connection.ExecuteAsync(sql,
                new
                {
                    @Id = id
                },
                _transaction);
            if (affectedRows > 0)
            {
                isSuccess = true;
            }

            return isSuccess;
        }

    }
}
