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
        bool Create(Post post);
        bool Update(Post post);
        bool Delete(int id);
    }
}
