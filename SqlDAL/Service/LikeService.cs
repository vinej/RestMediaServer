using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;

// simple service for future use 
namespace SqlDAL.Service
{
    public class LikeService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<Like>> _LikeManyCache = new WaitToFinishMemoryCache<IEnumerable<Like>>();
        static readonly WaitToFinishMemoryCache<Like> _LikeSingleCache = new WaitToFinishMemoryCache<Like>();

        public  long Insert(Like Like)
        {
            return  new LikeDal().Insert(Like);
        }

        public  long Update(Like Like)
        {
            return  new LikeDal().Update(Like);
        }

        public  long Delete(long id)
        {
            return  new LikeDal().Delete(id);
        }

        public Like GetById(long id)
        {
            return  _LikeSingleCache.GetOrCreate(id,  () =>  new LikeDal().GetById(id));
        }

        public IEnumerable<Like> GetAll()
        {
            return  _LikeManyCache.GetOrCreate("__all__",  () =>  new LikeDal().GetAll());
        }
    }
}
