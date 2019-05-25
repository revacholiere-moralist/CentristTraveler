using CentristTraveler.Models;
using CentristTraveler.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Implementations
{
    public class PostTagsRepository : BaseRepository, IPostTagsRepository
    {
        
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
            if (tags.Count == 0)
            {
                isSuccess = true;
            }
            foreach (Tag tag in tags)
            {
                try
                {
                    int affectedRows = _connection.Execute(sql,
                        new
                        {
                            @PostId = postId,
                            @TagId = tag.Id,
                            @CreatedBy = post.CreatedBy,
                            @CreatedDate = DateTime.Now,
                            @UpdatedBy = post.UpdatedBy,
                            @UpdatedDate = DateTime.Now
                        },
                        _transaction);
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

        public bool DeletePostTags(int postId, List<Tag> tags)
        {
            string sql = @"DELETE FROM [dbo].[Mapping_Post_Tag]
                            WHERE PostId = @PostId
                            AND TagId = @TagId";
            bool isSuccess = false;

            if (tags.Count == 0)
            {
                isSuccess = true;
            }
            foreach (Tag tag in tags)
            {
                try
                {
                    int affectedRows = _connection.Execute(sql,
                        new
                        {
                            @PostId = postId,
                            @TagId = tag.Id
                        },
                        _transaction);
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

        public bool DeletePostTagsByPostId(int postId)
        {
            string sql = @"DELETE FROM [dbo].[Mapping_Post_Tag]
                            WHERE PostId = @PostId";
            bool isSuccess = false;
            
            int affectedRows = _connection.Execute(sql,
                new
                {
                    @PostId = postId
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
