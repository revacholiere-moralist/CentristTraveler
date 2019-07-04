using CentristTraveler.BusinessLogic.Interfaces;
using CentristTraveler.Dto;
using CentristTraveler.Models;
using CentristTraveler.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        public List<PostDto> GetLatestPosts()
        {
            _postUoW.Begin();
            try
            {
                List<Post> posts = _postUoW.PostRepository.GetLatestPosts();
                List<PostDto> postDtos = new List<PostDto>();
                foreach (Post post in posts)
                {
                    List<Tag> tags = _postUoW.TagRepository.GetTagsByPostId(post.PostId);
                    User user = _postUoW.UserRepository.GetUserById(post.AuthorId);
                    PostDto postDto = new PostDto
                    {
                        Id = post.PostId,
                        Title = post.Title,
                        Body = post.Body,
                        Tags = tags,
                        ThumbnailPath = post.ThumbnailPath,
                        BannerPath = post.BannerPath,
                        PreviewText = post.PreviewText,
                        AuthorDisplayName = user.DisplayName,
                        CategoryId = post.CategoryId,
                        Views = post.Views,
                        Slug = post.Slug,
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
        public List<PostDto> SearchPosts(PostSearchParamDto searchParam)
        {
            var postDtos = new List<PostDto>();
            _postUoW.Begin();
            try
            {
                List<Post> posts = _postUoW.PostRepository.SearchPosts(searchParam);
                foreach (Post post in posts)
                {
                    List<Tag> tags = _postUoW.TagRepository.GetTagsByPostId(post.PostId);
                    User user = _postUoW.UserRepository.GetUserById(post.AuthorId);
                    PostDto postDto = new PostDto
                    {
                        Id = post.PostId,
                        Title = post.Title,
                        Body = post.Body,
                        Tags = tags,
                        ThumbnailPath = post.ThumbnailPath,
                        BannerPath = post.BannerPath,
                        PreviewText = post.PreviewText,
                        AuthorDisplayName = user.DisplayName,
                        CategoryId = post.CategoryId,
                        Views = post.Views,
                        Slug = post.Slug,
                        CreatedBy = post.CreatedBy,
                        CreatedDate = post.CreatedDate,
                        UpdatedBy = post.UpdatedBy,
                        UpdatedDate = post.UpdatedDate
                    };
                    postDtos.Add(postDto);
                }
            }
            catch (Exception ex)
            {
                postDtos = new List<PostDto>();
            }
            return postDtos;
        }
        public PostDto GetPostById(int id)
        {
            _postUoW.Begin();
            try
            {
                Post post = _postUoW.PostRepository.GetPostById(id);
                List<Tag> tags = _postUoW.TagRepository.GetTagsByPostId(post.PostId);
                User user = _postUoW.UserRepository.GetUserById(post.AuthorId);
                PostDto postDto = new PostDto
                {
                    Id = post.PostId,
                    Title = post.Title,
                    Body = post.Body,
                    Tags = tags,
                    ThumbnailPath = post.ThumbnailPath,
                    BannerPath = post.BannerPath,
                    BannerText = post.BannerText,
                    PreviewText = post.PreviewText,
                    AuthorDisplayName = user.DisplayName,
                    CategoryId = post.CategoryId,
                    Views = post.Views,
                    Slug = post.Slug,
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

        public bool Create(PostDto postDto)
        {
            bool isSuccess = false;

            _postUoW.Begin();
            try
            {
                User user = _postUoW.UserRepository.GetUserByUsername(postDto.AuthorUsername);
                Post post = new Post
                {
                    Title = postDto.Title,
                    Body = postDto.Body,
                    ThumbnailPath = postDto.ThumbnailPath,
                    PreviewText = postDto.PreviewText,
                    CategoryId = postDto.CategoryId,
                    BannerPath = postDto.BannerPath,
                    BannerText = postDto.BannerText,
                    AuthorId = user.UserId,
                    Slug = SlugifyString(postDto.Title.ToLower()),
                    CreatedBy = postDto.AuthorUsername,
                    CreatedDate = DateTime.Now,
                    UpdatedBy = postDto.AuthorUsername,
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
                                TagId = newTagId,
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
                if (postDto.ThumbnailPath != null)
                {
                    oldPost.ThumbnailPath = postDto.ThumbnailPath;
                }
                if (postDto.BannerPath != null)
                {
                    oldPost.BannerPath = postDto.BannerPath;
                }
                oldPost.Slug = SlugifyString(postDto.Title.ToLower());
                oldPost.BannerText = postDto.BannerText;
                oldPost.CategoryId = postDto.CategoryId;
                oldPost.PreviewText = postDto.PreviewText;
                oldPost.UpdatedBy = postDto.AuthorUsername;
                oldPost.UpdatedDate = DateTime.Now;

                isSuccess = _postUoW.PostRepository.Update(oldPost);
                List<Tag> newTags = new List<Tag>();
                List<Tag> deletedTags = new List<Tag>();
                if (isSuccess)
                {
                    List<Tag> tags = _postUoW.TagRepository.GetTagsByPostId(oldPost.PostId);
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
                                TagId = newTagId,
                                Name = newTag.Name
                            };
                            newTags.Add(tag);
                        }

                        else if (!oldTagsName.Contains(newTag.Name))
                        {
                            newTags.Add(newTag);
                        }
                    }

                    isSuccess = _postUoW.PostTagsRepository.InsertPostTags(oldPost.PostId, newTags, oldPost);
                    if (isSuccess)
                    {
                        isSuccess = _postUoW.PostTagsRepository.DeletePostTags(oldPost.PostId, deletedTags);
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

        public List<CategoryDto> GetAllCategories()
        {
            _postUoW.Begin();
            try
            {
                var categoryDtos = new List<CategoryDto>();
                var categories = _postUoW.CategoryRepository.GetAll();
                foreach(Category category in categories)
                {
                    var categoryDto = new CategoryDto
                    {
                        Id = category.CategoryId,
                        Name = category.Name
                    };
                    categoryDtos.Add(categoryDto);
                }
                return categoryDtos;
            }
            catch
            {
                return new List<CategoryDto>();
            }
        }
        public List<TagDto> GetPopularTags()
        {
            _postUoW.Begin();
            try
            {
                var tagDtos = new List<TagDto>();
                var tags = _postUoW.TagRepository.GetPopularTags();
                foreach (Tag tag in tags)
                {
                    var tagDto = new TagDto 
                    {
                        Id = tag.TagId,
                        Name = tag.Name
                    };
                    tagDtos.Add(tagDto);
                }
                return tagDtos;
            }
            catch
            {
                return new List<TagDto>();
            }
        }

        private string SlugifyString(string input)
        {
            input = Regex.Replace(input, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            input = Regex.Replace(input, @"\s+", " ").Trim();
            // cut and trim 
            input = input.Substring(0, input.Length <= 45 ? input.Length : 45).Trim();
            input = Regex.Replace(input, @"\s", "-"); // hyphens  
            return input;
        }

        public List<PostDto> GetPopularPosts()
        {
            _postUoW.Begin();
            try
            {
                List<Post> posts = _postUoW.PostRepository.GetPopularPosts();
                var postDtos = new List<PostDto>();
                foreach (var post in posts)
                {
                    PostDto postDto = new PostDto
                    {
                        Id = post.PostId,
                        Title = post.Title,
                        Body = post.Body,
                        ThumbnailPath = post.ThumbnailPath,
                        BannerPath = post.BannerPath,
                        PreviewText = post.PreviewText,
                        CategoryId = post.CategoryId,
                        Views = post.Views,
                        Slug = post.Slug,
                        CreatedBy = post.CreatedBy,
                        CreatedDate = post.CreatedDate,
                        UpdatedBy = post.UpdatedBy,
                        UpdatedDate = post.UpdatedDate
                    };
                    postDtos.Add(postDto);
                }
                return postDtos;
            }
            catch
            {
                return new List<PostDto>();
            }
        }
    }
}
