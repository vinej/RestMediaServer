using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;
using System.Threading.Tasks;

// simple service for future use 
namespace SqlDAL.Service
{
    public class VideoService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<Video>> _VideoManyCache = new WaitToFinishMemoryCache<IEnumerable<Video>>();
        static readonly WaitToFinishMemoryCache<Video> _VideoSingleCache = new WaitToFinishMemoryCache<Video>();

        public async Task<long> Insert(Video Video)
        {
            return await new VideoDal().Insert(Video);
        }

        public async Task<long> Update(Video Video)
        {
            return await new VideoDal().Update(Video);
        }

        public async Task<long> Delete(long id)
        {
            return await new VideoDal().Delete(id);
        }

        public async Task<Video> GetById(long id)
        {
            return await _VideoSingleCache.GetOrCreate(id, async () => await new VideoDal().GetById(id));
        }

        public async Task<IEnumerable<Video>> GetAll()
        {
            return await _VideoManyCache.GetOrCreate("__all__", async () => await new VideoDal().GetAll());
        }
    }
}
