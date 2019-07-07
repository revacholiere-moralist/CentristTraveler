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
    public class UserRepository : BaseRepository, IUserRepository
    {

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            string sql = @"SELECT [Id]
                          ,[Username]
                          ,[Email]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] FROM User";
            
            try
            {
                return await _connection.QueryAsync<User>(sql, null, _transaction);
            }
            catch (Exception ex)
            {
                return new List<User>();
                // TODO: Add Error Log
            }
        }

        public async Task<User> GetUserById(int id)
        {
            string sql = @"SELECT [Id] AS UserId
                          ,[Username]
                          ,[Email]
                          ,[DisplayName]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] 
                            FROM [dbo].[User]
                            WHERE Id = @Id";
            
            try
            {
                return await _connection.QueryFirstOrDefaultAsync<User>(sql,
                    new
                    {
                        @Id = id
                    }, 
                    _transaction);
            }
            catch (Exception ex)
            {
                return new User();
                // TODO: Add Error Log
            }

        }

        public async Task<User> GetUserByUsername(string username)
        {
            string sql = @"SELECT [Id] AS UserId
                          ,[Username]
                          ,[Email]
                          ,[DisplayName]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] 
                            FROM [dbo].[User]
                            WHERE Username = @username";
            try
            {
                return await _connection.QueryFirstOrDefaultAsync<User>(sql,
                    new
                    {
                        @Username = username
                    },
                    _transaction);
            }
            catch (Exception ex)
            {
                return new User();
                // TODO: Add Error Log
            }

        }

        public async Task<User> GetUserByEmail(string email)
        {
            string sql = @"SELECT [Id]
                          ,[Username]
                          ,[Email]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] 
                            FROM User
                            WHERE Email = @Email";
            try
            {
                return await _connection.QueryFirstOrDefaultAsync<User>(sql,
                    new
                    {
                        @Email = email
                    },
                    _transaction);
            }
            catch (Exception ex)
            {
                return new User();
                // TODO: Add Error Log
            }

        }

        public async Task<string> GetHashedPassword(string login)
        {
            string sql = @"SELECT [Password]
                            FROM [dbo].[User]
                            WHERE (Username = @login OR Email = @login)";
            
            try
            {
                return await _connection.ExecuteScalarAsync<string>(sql,
                    new
                    {
                        @login = login
                    },
                    _transaction);
            }
            catch (Exception ex)
            {
                return string.Empty;
                // TODO: Add Error Log
            }
        }

        public async Task<User> GetUserByLogin(string login)
        {
            string sql = @"SELECT [Id] AS UserId
                            ,[Username]
                            FROM [dbo].[User]
                            WHERE (Username = @login OR Email = @login)";
            
            try
            {
                return await _connection.QueryFirstOrDefaultAsync<User>(sql,
                    new
                    {
                        @login = login
                    },
                    _transaction);
            }
            catch (Exception ex)
            {
                return new User();
                // TODO: Add Error Log
            }
        }

        public async Task<int> Create(User user)
        {
            string sql = @"INSERT INTO [dbo].[User]
                           ([Username]
                           ,[Password]
                           ,[Email]
                           ,[DisplayName]
                           ,[CreatedBy]
                           ,[CreatedDate]
                           ,[UpdatedBy]
                           ,[UpdatedDate])
                     VALUES
                           (@Username
                           ,@Password
                           ,@Email
                           ,@DisplayName
                           ,@CreatedBy
                           ,@CreatedDate
                           ,@UpdatedBy
                           ,@UpdatedDate)
                SELECT CAST(SCOPE_IDENTITY() as int)";

            return await _connection.ExecuteScalarAsync<int>(sql,
                new
                {
                    @Username = user.Username,
                    @Password = user.Password,
                    @Email = user.Email,
                    @DisplayName = user.DisplayName,
                    @CreatedBy = user.CreatedBy,
                    @CreatedDate = user.CreatedDate,
                    @UpdatedBy = user.UpdatedBy,
                    @UpdatedDate = user.UpdatedDate
                },
               _transaction);
        }

        public async Task<bool> Update(User user)
        {
            string sql = @"UPDATE [dbo].[User]
                           SET [Password] = @Password
                              ,[DisplayName] = @DisplayName
                              ,[Email] = @Email
                              ,[UpdatedBy] = @UpdatedBy
                              ,[UpdatedDate] = @UpdatedDate
                         WHERE Id = @Id";
            bool isSuccess = false;

            int affectedRows = await _connection.ExecuteAsync(sql,
                new
                {
                    @Password = user.Password,
                    @Email = user.Email,
                    @DisplayName = user.DisplayName,
                    @UpdatedBy = user.UpdatedBy,
                    @UpdatedDate = user.UpdatedDate
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
            string sql = @"DELETE FROM [dbo].[User]
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
