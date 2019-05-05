using CentristTraveler.BusinessLogic.Interfaces;
using CentristTraveler.Dao.Interfaces;
using CentristTraveler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.BusinessLogic.Implementations
{
    public class PostBL : IPostBL
    {
        private IPostDao _postDao;
        
        public PostBL(IPostDao postDao)
        {
            _postDao = postDao;
        }
        public List<Post> GetAllPosts()
        {
            return _postDao.GetAllPosts();
        }

        public Post GetPostById(int id)
        {
            return _postDao.GetPostById(id);
        }

        public List<Post> GetPostsByCollaborators(List<string> username)
        {
            throw new NotImplementedException();
        }

        public List<Post> GetPostsByCreationDate(DateTime beginDate, DateTime endDate)
        {
            return GetPostsByCreationDate(beginDate, endDate);
        }

        public bool Create(Post post)
        {
            post.CreatedDate = DateTime.Now;
            post.CreatedBy = "admin";
            post.UpdatedDate = DateTime.Now;
            post.UpdatedBy = "admin";
            return _postDao.Create(post);
        }

        public bool Update(Post post)
        {
            post.UpdatedDate = DateTime.Now;
            post.UpdatedBy = "admin";
            return _postDao.Update(post);
        }

        public bool Delete(int id)
        {
            return _postDao.Delete(id);
        }

    }
}
