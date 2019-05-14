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
    public class PostRepository : IPostRepository
    {
        #region Private members
        private IDbTransaction _transaction;
        private IDbConnection _connection;
        #endregion
        #region Constructor
        public PostRepository(IDbTransaction transaction)
        {
            _transaction = transaction;
            _connection = transaction.Connection;
        }
        #endregion

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

            try
            {
                posts = _connection.Query<Post>(sql, null, _transaction).ToList();
            }
            catch
            {
                posts = new List<Post>();
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
            
            try
            {
                post = _connection.QueryFirstOrDefault<Post>(sql,
                    new
                    {
                        @Id = id
                    },
                    _transaction);
            }
            catch
            {
                post = new Post();
                //TODO: Add error log
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
            
            try
            {
                posts = _connection.Query<Post>(sql,
                    new
                    {
                        @BeginDate = beginDate,
                        @EndDate = endDate
                    },
                    _transaction).ToList();
            }
            catch
            {
                posts = new List<Post>();
                //TODO: Add error log
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
          
            postId = _connection.ExecuteScalar<int>(sql,
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
                _transaction);
            
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
             
            int affectedRows = _connection.Execute(sql,
                        new
                        {
                            @Id = post.Id,
                            @Title = post.Title,
                            @Body = post.Body,
                            @ThumbnailPath = post.ThumbnailPath,
                            @UpdatedDate = post.UpdatedDate,
                            @UpdatedBy = post.UpdatedBy
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
            string sql = @"DELETE FROM [dbo].[Post]
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
