using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;
using System.Threading.Tasks;

// simple service for future use 
namespace SqlDAL.Service
{
    public class OpinionService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<Opinion>> _OpinionManyCache = new WaitToFinishMemoryCache<IEnumerable<Opinion>>();
        static readonly WaitToFinishMemoryCache<Opinion> _OpinionSingleCache = new WaitToFinishMemoryCache<Opinion>();

        public async Task<long> Insert(Opinion Opinion)
        {
            return await new OpinionDal().Insert(Opinion);
        }

        public async Task<long> Update(Opinion Opinion)
        {
            return await new OpinionDal().Update(Opinion);
        }

        public async Task<long> Delete(long id)
        {
            return await new OpinionDal().Delete(id);
        }

        public async Task<Opinion> GetById(long id)
        {
            return await _OpinionSingleCache.GetOrCreate(id, async () => await new OpinionDal().GetById(id));
        }

        public async Task<IEnumerable<Opinion>> GetAll()
        {
            return await _OpinionManyCache.GetOrCreate("__all__", async () => await new OpinionDal().GetAll());
        }
    }
}
