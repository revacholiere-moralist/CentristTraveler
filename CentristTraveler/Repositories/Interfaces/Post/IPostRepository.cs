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
        Task<IEnumerable<Post>> GetLatestPosts();
        Task<IEnumerable<Post>> SearchPosts(PostSearchParamDto searchParam);
        Task<Post> GetPostById(int id);
        Task<int> Create(Post post);
        Task<bool> Update(Post post);
        Task<bool> Delete(int id);
        Task<IEnumerable<Post>> GetPopularPosts();
    }
}
