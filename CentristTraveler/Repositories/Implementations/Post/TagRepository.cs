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
    public class TagRepository: BaseRepository, ITagRepository
    {
        public async Task<IEnumerable<Tag>> GetAllTags()
        {
            string sql = @"SELECT [Id] AS TagId
                          ,[Name]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] FROM Master_Tag";
            
            try
            {
                return await _connection.QueryAsync<Tag>(sql, null, _transaction);
            }
            catch
            {
                return new List<Tag>();
                // TODO: Add Error Log
            }
            
        }

        public async Task<IEnumerable<Tag>> GetTagsByPostId(int postId)
        {
            string sql = @"SELECT Tag.[Id] AS TagId
                          ,Tag.[Name]
                          ,Tag.[CreatedDate]
                          ,Tag.[CreatedBy]
                          ,Tag.[UpdatedDate]
                          ,Tag.[UpdatedBy] 
                            FROM Master_Tag AS Tag
                            JOIN Mapping_Post_Tag As PostTag On Tag.Id = PostTag.TagId
                            WHERE PostTag.PostId = @PostId";
            
            try
            {
                return await _connection.QueryAsync<Tag>(sql, new {
                    @PostId = postId
                }, _transaction);
            }
            catch
            {
                return new List<Tag>();
                // TODO: Add Error Log
            }
            
        }

        public async Task<IEnumerable<Tag>> GetPopularTags()
        {
            string sql = @"SELECT TOP 5 Tag.[Id] AS TagId
                          ,Tag.[Name]
                          ,Post.[Views]
                            FROM Master_Tag AS Tag
                            JOIN Mapping_Post_Tag As PostTag On Tag.Id = PostTag.TagId
                            JOIN Post As Post On PostTag.PostId = Post.Id
                            GROUP BY Tag.[Id], Tag.[Name], Post.Views
                            ORDER BY Post.[Views] DESC";
            
            try
            {
                return await _connection.QueryAsync<Tag>(sql, null, _transaction);
            }
            catch (Exception ex)
            {
                return new List<Tag>();
                // TODO: Add Error Log
            }

        }
        public async Task<Tag> GetTagById(int id)
        {
            string sql = @"SELECT [Id] AS TagId
                          ,[Name]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] FROM Master_Tag
                            WHERE Id = @Id";
            try
            {
                return await _connection.QueryFirstOrDefaultAsync<Tag>(sql, new
                {
                    @Id = id
                }, _transaction);
            }
            catch (Exception ex)
            {
                return new Tag();
                // TODO: Add Error Log
            }
            
        }

        public async Task<int> Create(Tag tag)
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

            return await _connection.ExecuteScalarAsync<int>(sql,
                new
                {
                    @Name = tag.Name,
                    @CreatedDate = tag.CreatedDate,
                    @CreatedBy = tag.CreatedBy,
                    @UpdatedDate = tag.UpdatedDate,
                    @UpdatedBy = tag.UpdatedBy
                },
               _transaction);
        }

        public async Task<bool> Update(Tag tag)
        {
            string sql = @"UPDATE [dbo].[Master_Tag]
                           SET [Name] = @Name
                              
                              ,[UpdatedBy] = @UpdatedBy
                              ,[UpdatedDate] = @UpdatedDate
                         WHERE Id = @Id";

            bool isSuccess = false;
            
            int affectedRows = await _connection.ExecuteAsync(sql,
                new
                {
                    @Id = tag.TagId,
                    @Name = tag.Name,
                    @UpdatedBy = tag.UpdatedBy,
                    @UpdatedDate = tag.UpdatedDate
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
            string sql = @"DELETE FROM [dbo].[Master_Tag]
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

        public async Task<Tag> GetTagByName(string name)
        {
            string sql = @"SELECT [Id] AS TagId
                          ,[Name]
                          ,[CreatedDate]
                          ,[CreatedBy]
                          ,[UpdatedDate]
                          ,[UpdatedBy] FROM Master_Tag
                            WHERE Name = @Name";
            
            return await _connection.QueryFirstOrDefaultAsync<Tag>(sql, 
                new {
                    @Name = name
                },
                _transaction);
        }
    }

}
