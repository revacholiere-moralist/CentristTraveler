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
using CentristTraveler.Dto;

namespace CentristTraveler.Repositories.Implementations
{
    public class PostRepository : BaseRepository, IPostRepository
    {
        
        public List<Post> GetLatestPosts()
        {
            var sql = @"SELECT TOP 5 [Id] AS PostId
                          ,[Title]
                          ,[Body]
                          ,[ThumbnailPath]
                          ,[PreviewText]
                          ,[BannerPath]
                          ,[BannerText]
                          ,[AuthorId]
                          ,[CategoryId]
                          ,[Views]
                          ,[Slug]
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

        public List<Post> SearchPosts(PostSearchParamDto searchParam)
        {
            string sql = @"SELECT DISTINCT Post.[Id] As PostId
                          ,Post.[Title]
                          ,Post.[Body]
                          ,Post.[ThumbnailPath]
                          ,Post.[AuthorId]
                          ,Post.[PreviewText]
                          ,Post.[BannerPath]
                          ,Post.[BannerText]
                          ,Post.[CategoryId]
                          ,[Views]
                          ,[Slug]
                          ,Post.[CreatedDate]
                          ,Post.[CreatedBy]
                          ,Post.[UpdatedDate]
                          ,Post.[UpdatedBy]
                            FROM Post AS Post
                            INNER JOIN Mapping_Post_Tag AS PostTag on Post.Id = PostTag.PostId
                            INNER JOIN Master_Tag AS Tag ON Tag.Id = PostTag.TagId
                            WHERE ((@Title = '' OR Post.Title LIKE @Title) OR
                                (@Body = '' OR Post.Body LIKE @Body)) AND
                                (@CategoryId = 0 OR Post.CategoryId = @CategoryId) AND
                                (@Tag = '' OR Tag.Name LIKE @Tag)
                               
                            ORDER BY UpdatedDate DESC";
            List<Post> posts = new List<Post>();

            try
            {
                var postDictionary = new Dictionary<int, Post>();
                posts = _connection.Query<Post>(sql,
                    new
                    {
                        @Title = "%" + searchParam.Title + "%",
                        @Body = "%" + searchParam.Body + "%",
                        @CategoryId = searchParam.CategoryId,
                        Tag = "%" + searchParam.Tag + "%"
                        
                    },
                    _transaction
                    ).ToList();
            }
            catch (Exception ex)
            {
                posts = new List<Post>();
            }
            return posts;
        }

        public Post GetPostById(int id)
        {
            string sql = @"
                           UPDATE Post
                            SET [Views] = [Views] + 1
                            WHERE Id = @Id

                           SELECT [Id] As PostId
                          ,[Title]
                          ,[Body]
                          ,[ThumbnailPath]
                          ,[PreviewText]
                          ,[AuthorId]
                          ,[BannerPath]
                          ,[BannerText]
                          ,[CategoryId]
                          ,[Views]
                          ,[Slug]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy]  FROM Post
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


        public int Create(Post post)
        {
            string sql = @"INSERT INTO [dbo].[Post]
                       ([Title]
                       ,[Body]
                       ,[ThumbnailPath]
                       ,[PreviewText]
                       ,[BannerPath]
                       ,[BannerText]
                       ,[AuthorId]
                       ,[CategoryId]
                       ,[Slug]
                       ,[CreatedDate]
                       ,[CreatedBy]
                       ,[UpdatedDate]
                       ,[UpdatedBy])
                 VALUES
                       (@Title
                       ,@Body
                       ,@ThumbnailPath
                       ,@PreviewText
                       ,@BannerPath
                       ,@BannerText
                       ,@AuthorId
                       ,@CategoryId
                       ,@Slug
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
                    @PreviewText = post.PreviewText,
                    @BannerPath = post.BannerPath,
                    @BannerText = post.BannerText,
                    @AuthorId = post.AuthorId,
                    @CategoryId = post.CategoryId,
                    @Slug = post.Slug,
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
                              ,[BannerPath] = @BannerPath
                              ,[BannerText] = @BannerText
                              ,[AuthorId] = @AuthorId
                              ,[PreviewText] = @PreviewText
                              ,[CategoryId] = @CategoryId
                              ,[Slug] = @Slug
                              ,[UpdatedDate] = @UpdatedDate
                              ,[UpdatedBy] = @UpdatedBy
                         WHERE Id = @Id";

            bool isSuccess = false;
             
            int affectedRows = _connection.Execute(sql,
                        new
                        {
                            @Id = post.PostId,
                            @Title = post.Title,
                            @Body = post.Body,
                            @ThumbnailPath = post.ThumbnailPath,
                            @BannerPath = post.BannerPath,
                            @BannerText = post.BannerText,
                            @AuthorId = post.AuthorId,
                            @Slug = post.Slug,
                            @PreviewText = post.PreviewText,
                            @CategoryId = post.CategoryId,
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

        public List<Post> GetPopularPosts()
        {
            var sql = @"SELECT TOP 5 [Id] AS PostId
                          ,[Title]
                          ,[Body]
                          ,[ThumbnailPath]
                          ,[PreviewText]
                          ,[BannerPath]
                          ,[BannerText]
                          ,[AuthorId]
                          ,[CategoryId]
                          ,[Views]
                          ,[Slug]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] FROM Post
                            ORDER BY [Views] DESC";

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
    }
}
