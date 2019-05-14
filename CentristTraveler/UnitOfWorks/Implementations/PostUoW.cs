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
    public class PostUoW : IPostUoW
    {
        #region Private Members
        private string _connectionString;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private IPostRepository _postRepository;
        private ITagRepository _tagRepository;
        private IPostTagsRepository _postTagsRepository;
        #endregion

        #region Constructor
        public PostUoW(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionString = connectionStrings.Value.DefaultConnection;
            _connection = new SqlConnection(_connectionString);
        }
        #endregion

        public IPostRepository PostRepository
        {
            get { return _postRepository ?? (_postRepository = new PostRepository(_transaction)); }
        }
        public ITagRepository TagRepository
        {
            get { return _tagRepository ?? (_tagRepository = new TagRepository(_transaction)); }
        }
        public IPostTagsRepository PostTagsRepository
        {
            get { return _postTagsRepository ?? (_postTagsRepository = new PostTagsRepository(_transaction)); }
        }
        public void Begin()
        {
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }
        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }

            ResetRepositories();
        }

        private void ResetRepositories()
        {
            _postRepository = null;
            _tagRepository = null;
            _postTagsRepository = null;
        }
    }
}
