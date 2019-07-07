using CentristTraveler.Dto;
using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.BusinessLogic.Interfaces
{
    public interface IPostBL
    {
        Task<IEnumerable<PostDto>> GetLatestPosts();
        Task<IEnumerable<PostDto>> SearchPosts(PostSearchParamDto searchParam);
        
        Task<PostDto> GetPostById(int id);
        Task<bool> Create(PostDto postDto);
        Task<bool> Update(PostDto postDto);
        Task<bool> Delete(int id);
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<IEnumerable<TagDto>> GetPopularTags();
        Task<IEnumerable<PostDto>> GetPopularPosts();
    }
}
