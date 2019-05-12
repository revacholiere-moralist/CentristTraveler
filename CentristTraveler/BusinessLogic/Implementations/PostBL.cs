using CentristTraveler.BusinessLogic.Interfaces;
using CentristTraveler.Dao.Interfaces;
using CentristTraveler.Dto;
using CentristTraveler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace CentristTraveler.BusinessLogic.Implementations
{
    public class PostBL : IPostBL
    {
        private IPostDao _postDao;
        private ITagDao _tagDao;
        public PostBL(IPostDao postDao, ITagDao tagDao)
        {
            _postDao = postDao;
            _tagDao = tagDao;
        }
        public List<PostDto> GetAllPosts()
        {
            List<Post> posts = _postDao.GetAllPosts();
            
            List<PostDto> postDtos = new List<PostDto>();
            foreach (Post post in posts)
            {
                List<Tag> tags = _tagDao.GetTagsByPostId(post.Id);
                PostDto postDto = new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    Tags = tags,
                    ThumbnailPath = post.ThumbnailPath,
                    CreatedBy = post.CreatedBy,
                    CreatedDate = post.CreatedDate,
                    UpdatedBy = post.UpdatedBy,
                    UpdatedDate = post.UpdatedDate
                };
                postDtos.Add(postDto);
            }
            return postDtos;
        }

        public PostDto GetPostById(int id)
        {
            Post post = _postDao.GetPostById(id);
            List<Tag> tags = _tagDao.GetTagsByPostId(post.Id);
            PostDto postDto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                Tags = tags,
                ThumbnailPath = post.ThumbnailPath,
                CreatedBy = post.CreatedBy,
                CreatedDate = post.CreatedDate,
                UpdatedBy = post.UpdatedBy,
                UpdatedDate = post.UpdatedDate
            };
            return postDto;
        }

        public List<PostDto> GetPostsByCollaborators(List<string> username)
        {
            throw new NotImplementedException();
        }

        public List<PostDto> GetPostsByCreationDate(DateTime beginDate, DateTime endDate)
        {
            return GetPostsByCreationDate(beginDate, endDate);
        }

        public bool Create(PostDto postDto)
        {
            bool isSuccess = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    Post post = new Post
                    {
                        Title = postDto.Title,
                        Body = postDto.Title,
                        ThumbnailPath = postDto.ThumbnailPath,
                        CreatedBy = "admin",
                        CreatedDate = DateTime.Now,
                        UpdatedBy = "admin",
                        UpdatedDate = DateTime.Now
                    };

                    int postId = _postDao.Create(post);
                    if (postId > 0)
                    {
                        List<Tag> tags = new List<Tag>();
                        foreach (Tag tag in postDto.Tags)
                        {
                            Tag existingTag = _tagDao.GetTagByName(tag.Name);
                            if (existingTag == null)
                            {
                                tag.CreatedDate = DateTime.Now;
                                tag.CreatedBy = "admin";
                                tag.UpdatedDate = DateTime.Now;
                                tag.UpdatedBy = "admin";
                                int newTagId = _tagDao.Create(tag);
                                Tag newTag = new Tag
                                {
                                    Id = newTagId,
                                    Name = tag.Name
                                };
                                tags.Add(newTag);
                            }
                            else
                            {
                                tags.Add(existingTag);
                            }
                            
                        }
                        isSuccess = _postDao.InsertPostTags(postId, tags, post);
                        if (isSuccess)
                        {
                            scope.Complete();
                        }
                        else
                        {
                            scope.Dispose();
                        } 
                    }
                    return isSuccess;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return false;
                }
            }
            
            
        }

        public bool Update(PostDto postDto)
        {
            bool isSuccess = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    Post oldPost = _postDao.GetPostById(postDto.Id);

                    oldPost.Title = postDto.Title;
                    oldPost.Body = postDto.Body;
                    oldPost.ThumbnailPath = postDto.ThumbnailPath;
                    oldPost.CreatedBy = "admin";
                    oldPost.CreatedDate = DateTime.Now;
                    oldPost.UpdatedBy = "admin";
                    oldPost.UpdatedDate = DateTime.Now;

                    isSuccess = _postDao.Update(oldPost);
                    List<Tag> newTags = new List<Tag>();
                    List<Tag> deletedTags = new List<Tag>();
                    if (isSuccess)
                    {
                        List<Tag> tags = _tagDao.GetTagsByPostId(oldPost.Id);
                        foreach (Tag tag in tags)
                        {
                            if (!postDto.Tags.Contains(tag))
                            {
                                deletedTags.Add(tag);
                            }
                        }
                        foreach (Tag newTag in postDto.Tags)
                        {
                            Tag existingTag = _tagDao.GetTagByName(newTag.Name);
                            if (existingTag == null)
                            {
                                newTag.CreatedDate = DateTime.Now;
                                newTag.CreatedBy = "admin";
                                newTag.UpdatedDate = DateTime.Now;
                                newTag.UpdatedBy = "admin";
                                int newTagId = _tagDao.Create(newTag);
                                Tag tag = new Tag
                                {
                                    Id = newTagId,
                                    Name = newTag.Name
                                };
                                newTags.Add(tag);
                            }
                            
                            else if (!tags.Contains(newTag))
                            {
                                newTags.Add(newTag);
                            }
                        }

                        isSuccess = _postDao.InsertPostTags(oldPost.Id, newTags, oldPost);
                        if (isSuccess)
                        {
                            isSuccess = _postDao.DeletePostTags(oldPost.Id, deletedTags);
                        }
                        if (isSuccess)
                        {
                            scope.Complete();
                        }
                        else
                        {
                            scope.Dispose();
                        }
                    }
                    return isSuccess;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return false;
                }
            }
        }
        public bool Delete(int id)
        {
            bool isSuccess = _postDao.Delete(id);
            if (isSuccess)
            {
                isSuccess = _postDao.DeletePostTagsByPostId(id);
            }
            return isSuccess;
        }

    }
}
