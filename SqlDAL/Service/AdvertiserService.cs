using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;
using System.Threading.Tasks;

// simple service for future use 
namespace SqlDAL.Service
{
    public class AdvertiserService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<Advertiser>> _AdvertiserManyCache = new WaitToFinishMemoryCache<IEnumerable<Advertiser>>();
        static readonly WaitToFinishMemoryCache<Advertiser> _AdvertiserSingleCache = new WaitToFinishMemoryCache<Advertiser>();

        public async Task<long> Insert(Advertiser Advertiser)
        {
            return await new AdvertiserDal().Insert(Advertiser);
        }

        public async Task<long> Update(Advertiser Advertiser)
        {
            return await new AdvertiserDal().Update(Advertiser);
        }

        public async Task<long> Delete(long id)
        {
            return await new AdvertiserDal().Delete(id);
        }

        public async Task<Advertiser> GetById(long id)
        {
            return await _AdvertiserSingleCache.GetOrCreate(id, async () => await new AdvertiserDal().GetById(id));
        }

        public async Task<IEnumerable<Advertiser>> GetAll()
        {
            return await _AdvertiserManyCache.GetOrCreate("__all__", async () => await new AdvertiserDal().GetAll());
        }
    }
}
