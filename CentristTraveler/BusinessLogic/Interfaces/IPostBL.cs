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
        List<PostDto> GetLatestPosts();
        List<PostDto> SearchPosts(PostSearchParamDto searchParam);
        
        PostDto GetPostById(int id);
        bool Create(PostDto postDto);
        bool Update(PostDto postDto);
        bool Delete(int id);
        List<CategoryDto> GetAllCategories();
        List<TagDto> GetPopularTags();
        List<PostDto> GetPopularPosts();
    }
}
