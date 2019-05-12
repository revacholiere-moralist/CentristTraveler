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
    public class PostDao : IPostDao
    {
        private string _connectionString;
        public PostDao(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionString = connectionStrings.Value.DefaultConnection;
        }
        public List<Post> GetAllPosts()
        {
            string sql = @"SELECT [Id]
                          ,[Title]
                          ,[Body]
                          ,[ThumbnailPath]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] FROM Post
                            ORDER BY UpdatedDate DESC";
            List<Post> posts = new List<Post>();

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    posts = connection.Query<Post>(sql).ToList();
                }
                catch
                {
                    posts = new List<Post>();
                    // TODO: Add Error Log
                }
            }
            return posts;
        }

        public Post GetPostById(int id)
        {
            string sql = @"SELECT [Id]
                          ,[Title]
                          ,[Body]
                          ,[ThumbnailPath]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] FROM Post
                        WHERE Id = @Id";
            Post post = new Post();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    post = connection.QueryFirstOrDefault<Post>(sql,
                        new
                        {
                            @Id = id
                        });
                }
                catch
                {
                    post = new Post();
                    //TODO: Add error log
                }
            }
            return post;
        }

        public List<Post> GetPostsByCollaborators(List<string> username)
        {
            throw new NotImplementedException();
        }

        public List<Post> GetPostsByCreationDate(DateTime beginDate, DateTime endDate)
        {
            string sql = @"SELECT [Id]
                          ,[Title]
                          ,[Body]
                          ,[ThumbnailPath]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] FROM Post
                        WHERE CreatedDate >= @BeginDate AND CreatedDate <= @EndDate
                        ORDER BY CreatedDate";
            List<Post> posts = new List<Post>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    posts = connection.Query<Post>(sql,
                        new
                        {
                            @BeginDate = beginDate,
                            @EndDate = endDate
                        }).ToList();
                }
                catch
                {
                    posts = new List<Post>();
                    //TODO: Add error log
                }
            }
            return posts;
        }

        public int Create(Post post)
        {
            string sql = @"INSERT INTO [dbo].[Post]
                       ([Title]
                       ,[Body]
                       ,[ThumbnailPath]
                       ,[CreatedDate]
                       ,[CreatedBy]
                       ,[UpdatedDate]
                       ,[UpdatedBy])
                 VALUES
                       (@Title
                       ,@Body
                       ,@ThumbnailPath
                       ,@CreatedDate
                       ,@CreatedBy
                       ,@UpdatedDate
                       ,@UpdatedBy)
                SELECT CAST(SCOPE_IDENTITY() as int)";
            int postId = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        postId = connection.ExecuteScalar<int>(sql,
                            new
                            {
                                @Title = post.Title,
                                @Body = post.Body,
                                @ThumbnailPath = post.ThumbnailPath,
                                @CreatedDate = post.CreatedDate,
                                @CreatedBy = post.CreatedBy,
                                @UpdatedDate = post.UpdatedDate,
                                @UpdatedBy = post.UpdatedBy
                            },
                            transaction);
                        if (postId > 0)
                        {
                            transaction.Commit();
                        }
                    }

                    catch (Exception ex)
                    {
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
            return postId;
        }

        public bool Update(Post post)
        {
            string sql = @"UPDATE [dbo].[Post]
                           SET [Title] = @Title
                              ,[Body] = @Body
                              ,[ThumbnailPath] = @ThumbnailPath
                              ,[UpdatedDate] = @UpdatedDate
                              ,[UpdatedBy] = @UpdatedBy
                         WHERE Id = @Id";

            bool isSuccess = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int affectedRows = connection.Execute(sql,
                                    new
                                    {
                                        @Id = post.Id,
                                        @Title = post.Title,
                                        @Body = post.Body,
                                        @ThumbnailPath = post.ThumbnailPath,
                                        @UpdatedDate = post.UpdatedDate,
                                        @UpdatedBy = post.UpdatedBy
                                    },
                                    transaction);
                        if (affectedRows > 0)
                        {
                            isSuccess = true;
                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        isSuccess = false;
                        transaction.Rollback();
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
            string sql = @"DELETE FROM [dbo].[Post]
                WHERE Id = @Id";
            bool isSuccess = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
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
                    catch (Exception)
                    {
                        isSuccess = false;
                        transaction.Rollback();
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

        public bool InsertPostTags(int postId, List<Tag> tags, Post post)
        {
            string sql = @"INSERT INTO [dbo].[Mapping_Post_Tag]
                           ([PostId]
                           ,[TagId]
                           ,[CreatedBy]
                           ,[CreatedDate]
                           ,[UpdatedBy]
                           ,[UpdatedDate])
                     VALUES
                           (@PostId
                           ,@TagId
                           ,@CreatedBy
                           ,@CreatedDate
                           ,@UpdatedBy
                           ,@UpdatedDate)";
            bool isSuccess = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (Tag tag in tags)
                    {
                        try
                        {
                            int affectedRows = connection.Execute(sql,
                                new
                                {
                                    @PostId = postId,
                                    @TagId = tag.Id,
                                    @CreatedBy = post.CreatedBy,
                                    @CreatedDate = DateTime.Now,
                                    @UpdatedBy = post.UpdatedBy,
                                    @UpdatedDate = DateTime.Now
                                },
                                transaction);
                            if (affectedRows > 0)
                            {
                                isSuccess = true;
                            }
                        }

                        catch (Exception)
                        {
                            isSuccess = false;
                        }
                    }

                    if (isSuccess)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }

                    connection.Close();
                    connection.Dispose();
                }
            }
            return isSuccess;
        }

        public bool DeletePostTags(int postId, List<Tag> tags)
        {
            string sql = @"DELETE FROM [dbo].[Mapping_Post_Tag]
                            WHERE PostId = @PostId
                            AND TagId = @TagId";
            bool isSuccess = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (Tag tag in tags)
                    {
                        try
                        {
                            int affectedRows = connection.Execute(sql,
                                new
                                {
                                    @PostId = postId,
                                    @TagId = tag.Id
                                },
                                transaction);
                            if (affectedRows > 0)
                            {
                                isSuccess = true;
                            }
                        }

                        catch (Exception)
                        {
                            isSuccess = false;
                        }
                        
                    }
                    if (isSuccess)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Dispose();
                    }

                    connection.Close();
                    connection.Dispose();
                }
            }
            return isSuccess;
        }

        public bool DeletePostTagsByPostId(int postId)
        {
            string sql = @"DELETE FROM [dbo].[Mapping_Post_Tag]
                            WHERE PostId = @PostId";
            bool isSuccess = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {

                    try
                    {
                        int affectedRows = connection.Execute(sql,
                            new
                            {
                                @PostId = postId
                            },
                            transaction);
                        if (affectedRows > 0)
                        {
                            isSuccess = true;
                            transaction.Commit();
                        }
                    }

                    catch (Exception)
                    {
                        isSuccess = false;
                        transaction.Rollback();
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
    }

}
