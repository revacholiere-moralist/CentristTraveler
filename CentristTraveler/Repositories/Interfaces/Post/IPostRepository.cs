using CentristTraveler.Dto;
using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Interfaces
{
    public interface IPostRepository : IBaseRepository
    {
        List<Post> GetLatestPosts();
        List<Post> SearchPosts(PostSearchParamDto searchParam);
        Post GetPostById(int id);
        int Create(Post post);
        bool Update(Post post);
        bool Delete(int id);
        
    }
}
