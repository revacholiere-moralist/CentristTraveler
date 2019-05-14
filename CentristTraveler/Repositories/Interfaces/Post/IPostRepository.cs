using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Interfaces
{
    public interface IPostRepository
    {
        List<Post> GetAllPosts();
        List<Post> GetPostsByCreationDate(DateTime beginDate, DateTime endDate);
        List<Post> GetPostsByCollaborators(List<string> username);
        Post GetPostById(int id);
        int Create(Post post);
        bool Update(Post post);
        bool Delete(int id);
        
    }
}
