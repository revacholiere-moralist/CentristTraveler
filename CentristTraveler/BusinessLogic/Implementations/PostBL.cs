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
        public async Task<IEnumerable<PostDto>> GetLatestPosts()
        {
            _postUoW.Begin();
            try
            {
                IEnumerable<Post> posts = await _postUoW.PostRepository.GetLatestPosts();
                var postDtos = new List<PostDto>();
                foreach (Post post in posts)
                {
                    IEnumerable<Tag> tags = await _postUoW.TagRepository.GetTagsByPostId(post.PostId);
                    User user = await _postUoW.UserRepository.GetUserById(post.AuthorId);
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
        public async Task<IEnumerable<PostDto>> SearchPosts(PostSearchParamDto searchParam)
        {
            var postDtos = new List<PostDto>();
            _postUoW.Begin();
            try
            {
                IEnumerable<Post> posts = await _postUoW.PostRepository.SearchPosts(searchParam);
                foreach (Post post in posts)
                {
                    IEnumerable<Tag> tags = await _postUoW.TagRepository.GetTagsByPostId(post.PostId);
                    User user = await _postUoW.UserRepository.GetUserById(post.AuthorId);
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
        public async Task<PostDto> GetPostById(int id)
        {
            _postUoW.Begin();
            try
            {
                Post post = await _postUoW.PostRepository.GetPostById(id);
                IEnumerable<Tag> tags = await _postUoW.TagRepository.GetTagsByPostId(post.PostId);
                User user = await _postUoW.UserRepository.GetUserById(post.AuthorId);
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

        public async Task<bool> Create(PostDto postDto)
        {
            bool isSuccess = false;

            _postUoW.Begin();
            try
            {
                User user = await _postUoW.UserRepository.GetUserByUsername(postDto.AuthorUsername);
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

                int postId = await _postUoW.PostRepository.Create(post);
                if (postId > 0)
                {
                    List<Tag> tags = new List<Tag>();
                    foreach (Tag tag in postDto.Tags)
                    {
                        Tag existingTag = await _postUoW.TagRepository.GetTagByName(tag.Name);
                        if (existingTag == null)
                        {
                            tag.CreatedDate = DateTime.Now;
                            tag.CreatedBy = "admin";
                            tag.UpdatedDate = DateTime.Now;
                            tag.UpdatedBy = "admin";
                            int newTagId = await _postUoW.TagRepository.Create(tag);
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
                    isSuccess = await _postUoW.PostTagsRepository.InsertPostTags(postId, tags, post);
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

        public async Task<bool> Update(PostDto postDto)
        {
            bool isSuccess = false;

            _postUoW.Begin();
            try
            {
                Post oldPost = await _postUoW.PostRepository.GetPostById(postDto.Id);

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

                isSuccess = await _postUoW.PostRepository.Update(oldPost);
                List<Tag> newTags = new List<Tag>();
                List<Tag> deletedTags = new List<Tag>();
                if (isSuccess)
                {
                    IEnumerable<Tag> tags = await _postUoW.TagRepository.GetTagsByPostId(oldPost.PostId);
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
                        Tag existingTag = await _postUoW.TagRepository.GetTagByName(newTag.Name);
                        if (existingTag == null)
                        {
                            newTag.CreatedDate = DateTime.Now;
                            newTag.CreatedBy = "admin";
                            newTag.UpdatedDate = DateTime.Now;
                            newTag.UpdatedBy = "admin";
                            int newTagId = await _postUoW.TagRepository.Create(newTag);
                            Tag tag = new Tag
                            {
                                TagId = newTagId,
                                Name = newTag.Name
                            };
                            newTags.Add(tag);
                        }

                        else if (!oldTagsName.Contains(newTag.Name))
                        {
                            newTags.Add(existingTag);
                        }
                    }
                    
                    isSuccess = await _postUoW.PostTagsRepository.InsertPostTags(oldPost.PostId, newTags, oldPost);
                    if (isSuccess)
                    {
                        isSuccess = await _postUoW.PostTagsRepository.DeletePostTags(oldPost.PostId, deletedTags);
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
        public async Task<bool> Delete(int id)
        {
            bool isSuccess = false;
            _postUoW.Begin();
            try
            {
                isSuccess = await _postUoW.PostTagsRepository.DeletePostTagsByPostId(id); 
                if (isSuccess)
                {
                    isSuccess = await _postUoW.PostRepository.Delete(id);
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

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            _postUoW.Begin();
            try
            {
                var categoryDtos = new List<CategoryDto>();
                var categories = await _postUoW.CategoryRepository.GetAll();
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
        public async Task<IEnumerable<TagDto>> GetPopularTags()
        {
            _postUoW.Begin();
            try
            {
                var tagDtos = new List<TagDto>();
                var tags = await _postUoW.TagRepository.GetPopularTags();
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

        public async Task<IEnumerable<PostDto>> GetPopularPosts()
        {
            _postUoW.Begin();
            try
            {
                IEnumerable<Post> posts = await _postUoW.PostRepository.GetPopularPosts();
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

        private string SlugifyString(string input)
        {
            input = Regex.Replace(input, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            input = Regex.Replace(input, @"\s+", " ").Trim();
            input = Regex.Replace(input, @"\s", "-"); // hyphens  
            return input;
        }

    }
}
