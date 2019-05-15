using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Interfaces
{
    public interface IPostTagsRepository : IBaseRepository
    {
        bool InsertPostTags(int postId, List<Tag> tags, Post post);
        bool DeletePostTags(int postId, List<Tag> tags);
        bool DeletePostTagsByPostId(int postId);
    }
}
