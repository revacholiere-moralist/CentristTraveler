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
        #endregion

        #region Constructor
        public PostUoW( IOptions<ConnectionStrings> connectionString,
                        IPostRepository postRepository,
                        ITagRepository tagRepository,
                        IPostTagsRepository postTagsRepository) : base(connectionString)
        {
            
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _postTagsRepository = postTagsRepository;

        }
        #endregion

        public IPostRepository PostRepository
        {
            get { return _postRepository; }
        }
        public ITagRepository TagRepository
        {
            get { return _tagRepository; }
        }
        public IPostTagsRepository PostTagsRepository
        {
            get { return _postTagsRepository; }
        }
        public void Begin()
        {
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            _postRepository.CreateTransaction(_transaction);
            _tagRepository.CreateTransaction(_transaction);
            _postTagsRepository.CreateTransaction(_transaction);
        }
        
    }
}
