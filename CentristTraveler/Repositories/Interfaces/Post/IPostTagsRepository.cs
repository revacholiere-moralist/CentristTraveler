using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Interfaces
{
    public interface IPostTagsRepository : IBaseRepository
    {
        Task<bool> InsertPostTags(int postId, List<Tag> tags, Post post);
        Task<bool> DeletePostTags(int postId, List<Tag> tags);
        Task<bool> DeletePostTagsByPostId(int postId);
    }
}
