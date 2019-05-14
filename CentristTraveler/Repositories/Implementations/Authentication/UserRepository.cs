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
    public class UserRepository : IUserRepository
    {
        #region Private members
        private IDbTransaction _transaction;
        private IDbConnection _connection;
        #endregion
        public UserRepository(IDbTransaction transaction)
        {
            _transaction = transaction;
            _connection = transaction.Connection;
        }
        public List<User> GetAllUsers()
        {
            string sql = @"SELECT [Id]
                          ,[Username]
                          ,[Email]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] FROM User";
            List<User> users = new List<User>();

            try
            {
                users = _connection.Query<User>(sql, null, _transaction).ToList();
            }
            catch (Exception ex)
            {
                users = new List<User>();
                // TODO: Add Error Log
            }

            return users;
        }

        public User GetUserById(int id)
        {
            string sql = @"SELECT [Id]
                          ,[Username]
                          ,[Email]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] 
                            FROM User
                            WHERE Id = @Id";
            User user = new User();

            try
            {
                user = _connection.Query<User>(sql,
                    new
                    {
                        @Id = id
                    }, 
                    _transaction).FirstOrDefault();
            }
            catch (Exception ex)
            {
                user = new User();
                // TODO: Add Error Log
            }

            return user;
        }

        public User GetUserByUsername(string username)
        {
            string sql = @"SELECT [Id]
                          ,[Username]
                          ,[Email]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] 
                            FROM User
                            WHERE Id = @Username";
            User user = new User();

            try
            {
                user = _connection.Query<User>(sql,
                    new
                    {
                        @Username = username
                    },
                    _transaction).FirstOrDefault();
            }
            catch (Exception ex)
            {
                user = new User();
                // TODO: Add Error Log
            }

            return user;
        }

        public User GetUserByEmail(string email)
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
            User user = new User();

            try
            {
                user = _connection.Query<User>(sql,
                    new
                    {
                        @Email = email
                    },
                    _transaction).FirstOrDefault();
            }
            catch (Exception ex)
            {
                user = new User();
                // TODO: Add Error Log
            }

            return user;
        }

        public User GetUserByLoginAndPassword(string login, string password)
        {
            string sql = @"SELECT [Id]
                          ,[Username]
                          ,[Email]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] 
                            FROM User
                            WHERE (Username = @login OR Email = @login)
                            AND Password = @password";
            User user = new User();

            try
            {
                user = _connection.Query<User>(sql,
                    new
                    {
                        @login = login,
                        @password = password
                    },
                    _transaction).FirstOrDefault();
            }
            catch (Exception ex)
            {
                user = new User();
                // TODO: Add Error Log
            }
            return user;
        }

        public int Create(User user)
        {
            string sql = @"INSERT INTO [dbo].[User]
                           ([Username]
                           ,[Password]
                           ,[Email]
                           ,[CreatedBy]
                           ,[CreatedDate]
                           ,[UpdatedBy]
                           ,[UpdatedDate])
                     VALUES
                           (@Username
                           ,@Password
                           ,@Email
                           ,@CreatedBy
                           ,@CreatedDate
                           ,@UpdatedBy
                           ,@UpdatedDate)
                SELECT CAST(SCOPE_IDENTITY() as int)";

            return _connection.ExecuteScalar<int>(sql,
                new
                {
                    @Username = user.Username,
                    @Password = user.Password,
                    @Email = user.Email,
                    @CreatedBy = user.CreatedBy,
                    @CreatedDate = user.CreatedDate,
                    @UpdatedBy = user.UpdatedBy,
                    @UpdatedDate = user.UpdatedDate
                },
               _transaction);
        }

        public bool Update(User user)
        {
            string sql = @"UPDATE [dbo].[User]
                           SET [Password] = @Password
                              ,[Email] = @Email
                              ,[UpdatedBy] = @UpdatedBy
                              ,[UpdatedDate] = @UpdatedDate
                         WHERE Id = @Id";
            bool isSuccess = false;

            int affectedRows = _connection.Execute(sql,
                new
                {
                    @Password = user.Password,
                    @Email = user.Email,
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
        public bool Delete(int id)
        {
            string sql = @"DELETE FROM [dbo].[User]
                WHERE Id = @Id";
            bool isSuccess = false;

            int affectedRows = _connection.Execute(sql,
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
