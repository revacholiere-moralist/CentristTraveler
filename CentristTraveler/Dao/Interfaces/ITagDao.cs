using CentristTraveler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.Dao.Interfaces
{
    public interface ITagDao
    {
        List<Tag> GetAllTags();
        List<Tag> GetTagsByPostId(int postId);
        Tag GetTagById(int id);
        Tag GetTagByName(string name);
        int Create(Tag tag);
        bool Update(Tag tag);
        bool Delete(int id);
    }
}
