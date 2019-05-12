using CentristTraveler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Dao.Interfaces
{
    public interface IPostDao
    {
        List<Post> GetAllPosts();
        List<Post> GetPostsByCreationDate(DateTime beginDate, DateTime endDate);
        List<Post> GetPostsByCollaborators(List<string> username);
        Post GetPostById(int id);
        int Create(Post post);
        bool Update(Post post);
        bool Delete(int id);
        bool InsertPostTags(int postId, List<Tag> tags, Post post);
        bool DeletePostTags(int postId, List<Tag> tags);
        bool DeletePostTagsByPostId(int postId);
    }
}
