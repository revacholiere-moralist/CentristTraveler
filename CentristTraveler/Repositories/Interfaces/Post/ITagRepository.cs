using CentristTraveler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Repositories.Interfaces
{
    public interface ITagRepository : IBaseRepository
    {
        Task<IEnumerable<Tag>> GetAllTags();
        Task<IEnumerable<Tag>> GetTagsByPostId(int postId);
        Task<IEnumerable<Tag>> GetPopularTags();
        Task<Tag>GetTagById(int id);
        Task<Tag>GetTagByName(string name);
        Task<int> Create(Tag tag);
        Task<bool> Update(Tag tag);
        Task<bool> Delete(int id);
    }
}
