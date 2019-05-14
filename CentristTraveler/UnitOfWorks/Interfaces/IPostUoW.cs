using CentristTraveler.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CentristTraveler.UnitOfWorks.Interfaces
{
    public interface IPostUoW
    {
        IPostRepository PostRepository { get; }
        ITagRepository TagRepository { get; }
        IPostTagsRepository PostTagsRepository { get; }
        void Begin();
        void Commit();
        void Dispose();
    }
}
