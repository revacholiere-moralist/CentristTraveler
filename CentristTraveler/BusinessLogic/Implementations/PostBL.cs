using CentristTraveler.BusinessLogic.Interfaces;
using CentristTraveler.Dto;
using CentristTraveler.Models;
using CentristTraveler.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace CentristTraveler.BusinessLogic.Implementations
{
    public class PostBL : IPostBL
    {
        private IPostUoW _postUoW;
        public PostBL(IPostUoW postUoW)
        {
            _postUoW = postUoW;
        }
        public List<PostDto> GetAllPosts()
        {
            _postUoW.Begin();
            try
            {
                List<Post> posts = _postUoW.PostRepository.GetAllPosts();
                List<PostDto> postDtos = new List<PostDto>();
                foreach (Post post in posts)
                {
                    List<Tag> tags = _postUoW.TagRepository.GetTagsByPostId(post.Id);
                    PostDto postDto = new PostDto
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Body = post.Body,
                        Tags = tags,
                        ThumbnailPath = post.ThumbnailPath,
                        BannerPath = post.BannerPath,
                        PreviewText = post.PreviewText,
                        CategoryId = post.CategoryId,
                        CreatedBy = post.CreatedBy,
                        CreatedDate = post.CreatedDate,
                        UpdatedBy = post.UpdatedBy,
                        UpdatedDate = post.UpdatedDate
                    };
                    postDtos.Add(postDto);
                }
                _postUoW.Commit();
                return postDtos;
            }
            catch (Exception ex)
            {
                _postUoW.Dispose();
                return new List<PostDto>();
            }
            
        }

        public PostDto GetPostById(int id)
        {
            _postUoW.Begin();
            try
            {
                Post post = _postUoW.PostRepository.GetPostById(id);
                List<Tag> tags = _postUoW.TagRepository.GetTagsByPostId(post.Id);
                PostDto postDto = new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    Tags = tags,
                    ThumbnailPath = post.ThumbnailPath,
                    BannerPath = post.BannerPath,
                    BannerText = post.BannerText,
                    PreviewText = post.PreviewText,
                    CategoryId = post.CategoryId,
                    CreatedBy = post.CreatedBy,
                    CreatedDate = post.CreatedDate,
                    UpdatedBy = post.UpdatedBy,
                    UpdatedDate = post.UpdatedDate
                };
                _postUoW.Commit();
                return postDto;
            }

            catch (Exception ex)
            {
                _postUoW.Dispose();
                return new PostDto();
            }

        }

        public List<PostDto> GetPostsByCollaborators(List<string> username)
        {
            throw new NotImplementedException();
        }

        public List<PostDto> GetPostsByCreationDate(DateTime beginDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public bool Create(PostDto postDto)
        {
            bool isSuccess = false;

            _postUoW.Begin();
            try
            {
                Post post = new Post
                {
                    Title = postDto.Title,
                    Body = postDto.Title,
                    ThumbnailPath = postDto.ThumbnailPath,
                    PreviewText = postDto.PreviewText,
                    CategoryId = postDto.CategoryId,
                    BannerPath = postDto.BannerPath,
                    BannerText = postDto.BannerText,
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    UpdatedBy = "admin",
                    UpdatedDate = DateTime.Now
                };

                int postId = _postUoW.PostRepository.Create(post);
                if (postId > 0)
                {
                    List<Tag> tags = new List<Tag>();
                    foreach (Tag tag in postDto.Tags)
                    {
                        Tag existingTag = _postUoW.TagRepository.GetTagByName(tag.Name);
                        if (existingTag == null)
                        {
                            tag.CreatedDate = DateTime.Now;
                            tag.CreatedBy = "admin";
                            tag.UpdatedDate = DateTime.Now;
                            tag.UpdatedBy = "admin";
                            int newTagId = _postUoW.TagRepository.Create(tag);
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
                    isSuccess = _postUoW.PostTagsRepository.InsertPostTags(postId, tags, post);
                    if (isSuccess)
                    {
                        _postUoW.Commit();
                    }
                    else
                    {
                        _postUoW.Dispose();
                    }
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                _postUoW.Dispose();
                return false;
            }
        }

        public bool Update(PostDto postDto)
        {
            bool isSuccess = false;

            _postUoW.Begin();
            try
            {
                Post oldPost = _postUoW.PostRepository.GetPostById(postDto.Id);

                oldPost.Title = postDto.Title;
                oldPost.Body = postDto.Body;
                oldPost.ThumbnailPath = postDto.ThumbnailPath;
                oldPost.BannerPath = postDto.BannerPath;
                oldPost.BannerText = postDto.BannerText;
                oldPost.CategoryId = postDto.CategoryId;
                oldPost.PreviewText = postDto.PreviewText;
                oldPost.CreatedBy = "admin";
                oldPost.CreatedDate = DateTime.Now;
                oldPost.UpdatedBy = "admin";
                oldPost.UpdatedDate = DateTime.Now;

                isSuccess = _postUoW.PostRepository.Update(oldPost);
                List<Tag> newTags = new List<Tag>();
                List<Tag> deletedTags = new List<Tag>();
                if (isSuccess)
                {
                    List<Tag> tags = _postUoW.TagRepository.GetTagsByPostId(oldPost.Id);
                    List<string> newTagsName = postDto.Tags.Select(x => x.Name).ToList();
                    List<string> oldTagsName = tags.Select(x => x.Name).ToList();
                    foreach (Tag tag in tags)
                    {
                        if (!newTagsName.Contains(tag.Name))
                        {
                            deletedTags.Add(tag);
                        }
                    }
                    foreach (Tag newTag in postDto.Tags)
                    {
                        Tag existingTag = _postUoW.TagRepository.GetTagByName(newTag.Name);
                        if (existingTag == null)
                        {
                            newTag.CreatedDate = DateTime.Now;
                            newTag.CreatedBy = "admin";
                            newTag.UpdatedDate = DateTime.Now;
                            newTag.UpdatedBy = "admin";
                            int newTagId = _postUoW.TagRepository.Create(newTag);
                            Tag tag = new Tag
                            {
                                Id = newTagId,
                                Name = newTag.Name
                            };
                            newTags.Add(tag);
                        }

                        else if (!oldTagsName.Contains(newTag.Name))
                        {
                            newTags.Add(newTag);
                        }
                    }

                    isSuccess = _postUoW.PostTagsRepository.InsertPostTags(oldPost.Id, newTags, oldPost);
                    if (isSuccess)
                    {
                        isSuccess = _postUoW.PostTagsRepository.DeletePostTags(oldPost.Id, deletedTags);
                    }
                    if (isSuccess)
                    {
                        _postUoW.Commit();
                    }
                    else
                    {
                        _postUoW.Dispose();
                    }
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                _postUoW.Dispose();
                return false;
            }

        }
        public bool Delete(int id)
        {
            bool isSuccess = false;
            _postUoW.Begin();
            try
            {
                isSuccess = _postUoW.PostTagsRepository.DeletePostTagsByPostId(id); 
                if (isSuccess)
                {
                    isSuccess = _postUoW.PostRepository.Delete(id);
                    _postUoW.Commit();
                }
            }
            catch(Exception ex)
            {
                isSuccess = false;
                _postUoW.Dispose();
            }
            return isSuccess;
        }

        public List<Category> GetAllCategories()
        {
            _postUoW.Begin();
            try
            {
                return _postUoW.CategoryRepository.GetAll();
            }
            catch
            {
                return new List<Category>();
            }
        }
    }
}
