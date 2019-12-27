using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;
using System.Threading.Tasks;

// simple service for future use 
namespace SqlDAL.Service
{
    public class TopicService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<Topic>> _TopicManyCache = new WaitToFinishMemoryCache<IEnumerable<Topic>>();
        static readonly WaitToFinishMemoryCache<Topic> _TopicSingleCache = new WaitToFinishMemoryCache<Topic>();

        public async Task<long> Insert(Topic Topic)
        {
            return await new TopicDal().Insert(Topic);
        }

        public async Task<long> Update(Topic Topic)
        {
            return await new TopicDal().Update(Topic);
        }

        public async Task<long> Delete(long id)
        {
            return await new TopicDal().Delete(id);
        }

        public async Task<Topic> GetById(long id)
        {
            return await _TopicSingleCache.GetOrCreate(id, async () => await new TopicDal().GetById(id));
        }

        public async Task<IEnumerable<Topic>> GetAll()
        {
            return await _TopicManyCache.GetOrCreate("__all__", async () => await new TopicDal().GetAll());
        }
    }
}
