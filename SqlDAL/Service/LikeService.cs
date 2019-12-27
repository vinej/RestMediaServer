using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;
using System.Threading.Tasks;

// simple service for future use 
namespace SqlDAL.Service
{
    public class LikeService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<Like>> _LikeManyCache = new WaitToFinishMemoryCache<IEnumerable<Like>>();
        static readonly WaitToFinishMemoryCache<Like> _LikeSingleCache = new WaitToFinishMemoryCache<Like>();

        public async Task<long> Insert(Like Like)
        {
            return await new LikeDal().Insert(Like);
        }

        public async Task<long> Update(Like Like)
        {
            return await new LikeDal().Update(Like);
        }

        public async Task<long> Delete(long id)
        {
            return await new LikeDal().Delete(id);
        }

        public async Task<Like> GetById(long id)
        {
            return await _LikeSingleCache.GetOrCreate(id, async () => await new LikeDal().GetById(id));
        }

        public async Task<IEnumerable<Like>> GetAll()
        {
            return await _LikeManyCache.GetOrCreate("__all__", async () => await new LikeDal().GetAll());
        }
    }
}
