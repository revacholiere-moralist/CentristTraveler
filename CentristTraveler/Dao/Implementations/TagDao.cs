using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CentristTraveler.Dao.Interfaces;
using CentristTraveler.Helper;
using CentristTraveler.Model;
using Microsoft.Extensions.Options;

using Dapper;

namespace CentristTraveler.Dao.Implementations
{
    public class TagDao: ITagDao
    {
        private string _connectionString;
        public TagDao(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionString = connectionStrings.Value.DefaultConnection;
        }

        public List<Tag> GetAllTags()
        {
            string sql = @"SELECT [Id]
                          ,[Name]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] FROM Master_Tag";
            List<Tag> tags = new List<Tag>();

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    tags = connection.Query<Tag>(sql).ToList();
                }
                catch
                {
                    tags = new List<Tag>();
                    // TODO: Add Error Log
                }
            }
            return tags;
        }

        public List<Tag> GetTagsByPostId(int postId)
        {
            string sql = @"SELECT Tag.[Id]
                          ,Tag.[Name]
                          ,Tag.[CreatedDate]
                          ,Tag.[CreatedBy]
                          ,Tag.[UpdatedDate]
                          ,Tag.[UpdatedBy] 
                            FROM Master_Tag AS Tag
                            JOIN Mapping_Post_Tag As PostTag On Tag.Id = PostTag.TagId
                            WHERE PostTag.PostId = @PostId" ;
            List<Tag> tags = new List<Tag>();

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    tags = connection.Query<Tag>(sql, new {
                        @PostId = postId
                    }).ToList();
                }
                catch
                {
                    tags = new List<Tag>();
                    // TODO: Add Error Log
                }
            }
            return tags;
        }

        public Tag GetTagById(int id)
        {
            string sql = @"SELECT [Id]
                          ,[Name]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] FROM Master_Tag
                            WHERE Id = @Id";
            Tag tag = new Tag();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    tag = connection.Query<Tag>(sql, new
                    {
                        @Id = id
                    }).FirstOrDefault();
                }
                catch
                {
                    tag = null;
                    // TODO: Add Error Log
                }
            }
            return tag;
        }

        public int Create(Tag tag)
        {
            string sql = @"INSERT INTO [dbo].[Master_Tag]
                       ([Name]
                       ,[CreatedDate]
                       ,[CreatedBy]
                       ,[UpdatedDate]
                       ,[UpdatedBy])
                 VALUES
                       (@Name
                       ,@CreatedDate
                       ,@CreatedBy
                       ,@UpdatedDate
                       ,@UpdatedBy)
                SELECT CAST(SCOPE_IDENTITY() as int)";
            int newId = 0;
            bool isSuccess = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        newId = connection.ExecuteScalar<int>(sql,
                            new
                            {
                                @Name = tag.Name,
                                @CreatedDate = tag.CreatedDate,
                                @CreatedBy = tag.CreatedBy,
                                @UpdatedDate = tag.UpdatedDate,
                                @UpdatedBy = tag.UpdatedBy
                            },
                            transaction);
                        if (newId > 0)
                        {
                            isSuccess = true;
                            transaction.Commit();
                        }
                    }

                    catch (Exception ex)
                    {
                        isSuccess = false;
                        transaction.Rollback();
                        //TODO: add error log
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }

            }
            return newId;
        }

        public bool Update(Tag tag)
        {
            string sql = @"UPDATE [dbo].[Master_Tag]
                           SET [Name] = @Name
                              
                              ,[UpdatedBy] = @UpdatedBy
                              ,[UpdatedDate] = @UpdatedDate
                         WHERE Id = @Id";

            bool isSuccess = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int affectedRows = connection.Execute(sql,
                            new
                            {
                                @Id = tag.Id,
                                @Name = tag.Name,
                                @UpdatedBy = tag.UpdatedBy,
                                @UpdatedDate = tag.UpdatedDate
                            },
                            transaction);
                        if (affectedRows > 0)
                        {
                            isSuccess = true;
                            transaction.Commit();
                        }
                    }

                    catch
                    {
                        isSuccess = false;
                        transaction.Rollback();
                        //TODO: add error log
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }

            }
            return isSuccess;
        }

        public bool Delete(int id)
        {
            string sql = @"DELETE FROM [dbo].[Master_Tag]
                WHERE Id = @Id";
            bool isSuccess = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int affectedRows = connection.Execute(sql,
                            new
                            {
                                @Id = id
                            },
                            transaction);
                        if (affectedRows > 0)
                        {
                            isSuccess = true;
                            transaction.Commit();
                        }
                    }

                    catch
                    {
                        isSuccess = false;
                        transaction.Rollback();
                        //TODO: add error log
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }

            }
            return isSuccess;
        }

        public Tag GetTagByName(string name)
        {
            string sql = @"SELECT [Id]
                          ,[Name]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] FROM Master_Tag
                            WHERE Name = @Name";
            Tag tag = new Tag();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    tag = connection.Query<Tag>(sql, new
                    {
                        @Name = name
                    }).FirstOrDefault();
                }
                catch
                {
                    tag = null;
                    // TODO: Add Error Log
                }
            }
            return tag;
        }
    }

}
