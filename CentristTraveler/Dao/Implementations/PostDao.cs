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
    public class PostDao: IPostDao
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
                          ,[UpdatedBy] FROM Post";
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

        public bool Create(Post post)
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
                       ,@UpdatedBy)";

            bool isSuccess = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    int affectedRows = connection.Execute(sql,
                        new
                        {
                            @Title = post.Title,
                            @Body = post.Body,
                            @ThumbnailPath = post.ThumbnailPath,
                            @CreatedDate = post.CreatedDate,
                            @CreatedBy = post.CreatedBy,
                            @UpdatedDate = post.UpdatedDate,
                            @UpdatedBy = post.UpdatedBy
                        });
                    if (affectedRows > 0)
                    {
                        isSuccess = true;
                    }
                }
                catch
                {
                    isSuccess = false;
                    //TODO: add error log
                }
            }
            return isSuccess;
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
                                });
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
            return isSuccess;
        }


        public bool Delete(int id)
        {
            string sql = @"DELETE FROM [dbo].[Post]
                WHERE Id = @Id";
            bool isSuccess = false;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    int affectedRows = connection.Execute(sql,
                        new
                        {
                            @Id = id
                        });
                    if (affectedRows > 0)
                    {
                        isSuccess = true;
                    }
                }
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            return isSuccess;
        }
    }

}
