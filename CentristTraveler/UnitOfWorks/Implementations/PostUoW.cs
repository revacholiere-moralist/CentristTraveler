using CentristTraveler.Helper;
using CentristTraveler.Repositories.Implementations;
using CentristTraveler.Repositories.Interfaces;
using CentristTraveler.UnitOfWorks.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.UnitOfWorks.Implementations
{
    public class PostUoW : BaseUoW, IPostUoW
    {
        #region Private Members
        private IPostRepository _postRepository;
        private ITagRepository _tagRepository;
        private IPostTagsRepository _postTagsRepository;
        private ICategoryRepository _categoryRepository;
        private IUserRepository _userRepository;
        #endregion

        #region Constructor
        public PostUoW(IOptions<ConnectionStrings> connectionString,
                        IPostRepository postRepository,
                        ITagRepository tagRepository,
                        IPostTagsRepository postTagsRepository,
                        ICategoryRepository categoryRepository,
                        IUserRepository userRepository) : base(connectionString)
        {

            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _postTagsRepository = postTagsRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }
        #endregion

        public IPostRepository PostRepository
        {
            get
            {
                _postRepository.CreateTransaction(_transaction);
                return _postRepository;
            }
        }
        public ITagRepository TagRepository
        {
            get
            {
                _tagRepository.CreateTransaction(_transaction);
                return _tagRepository;
            }
        }
        public IPostTagsRepository PostTagsRepository
        {
            get
            {
                _postTagsRepository.CreateTransaction(_transaction);
                return _postTagsRepository;
            }
        }
        public ICategoryRepository CategoryRepository
        {
            get
            {
                _categoryRepository.CreateTransaction(_transaction);
                return _categoryRepository;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                _userRepository.CreateTransaction(_transaction);
                return _userRepository;
            }
        }

        public void Begin()
        {
            _connection.Open();
            _transaction = _connection.BeginTransaction();

        }

    }
}
