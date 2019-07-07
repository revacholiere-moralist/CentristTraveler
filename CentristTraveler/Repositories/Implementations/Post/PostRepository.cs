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
        
        public async Task<IEnumerable<Post>> GetLatestPosts()
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
            
            try
            {
                return await _connection.QueryAsync<Post>(sql, null, _transaction);
            }
            catch
            {
                return new List<Post>();
            }
        }

        public async Task<IEnumerable<Post>> SearchPosts(PostSearchParamDto searchParam)
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
            
            try
            {
                return await _connection.QueryAsync<Post>(sql,
                    new
                    {
                        @Title = "%" + searchParam.Title + "%",
                        @Body = "%" + searchParam.Body + "%",
                        @CategoryId = searchParam.CategoryId,
                        Tag = "%" + searchParam.Tag + "%"
                        
                    },
                    _transaction
                    );
            }
            catch (Exception ex)
            {
                return new List<Post>();
            }
        }

        public async Task<Post> GetPostById(int id)
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
            
            try
            {
                return await _connection.QueryFirstOrDefaultAsync<Post>(sql,
                    new
                    {
                        @Id = id
                    },
                    _transaction);
            }
            catch
            {
                return new Post();
                //TODO: Add error log
            }
            
        }


        public async Task<int> Create(Post post)
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
            
            return await _connection.ExecuteScalarAsync<int>(sql,
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
        }

        public async Task<bool> Update(Post post)
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
             
            int affectedRows = await _connection.ExecuteAsync(sql,
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


        public async Task<bool> Delete(int id)
        {
            string sql = @"DELETE FROM [dbo].[Post]
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

        public async Task<IEnumerable<Post>> GetPopularPosts()
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

            try
            {
                return await _connection.QueryAsync<Post>(sql, null, _transaction);
            }
            catch
            {
                return new List<Post>();
            }
        }
    }
}
