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
        List<PostDto> GetAllPosts();
        List<PostDto> GetPostsByCreationDate(DateTime beginDate, DateTime endDate);
        List<PostDto> GetPostsByCollaborators(List<string> username);
        PostDto GetPostById(int id);
        bool Create(PostDto postDto);
        bool Update(PostDto postDto);
        bool Delete(int id);
    }
}
