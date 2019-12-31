using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;

// simple service for future use 
namespace SqlDAL.Service
{
    public class AdvertiserService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<Advertiser>> _AdvertiserManyCache = new WaitToFinishMemoryCache<IEnumerable<Advertiser>>();
        static readonly WaitToFinishMemoryCache<Advertiser> _AdvertiserSingleCache = new WaitToFinishMemoryCache<Advertiser>();

        public  long Insert(Advertiser Advertiser)
        {
            return  new AdvertiserDal().Insert(Advertiser);
        }

        public  long Update(Advertiser Advertiser)
        {
            return  new AdvertiserDal().Update(Advertiser);
        }

        public  long Delete(long id)
        {
            return  new AdvertiserDal().Delete(id);
        }

        public  Advertiser GetById(long id)
        {
            return  _AdvertiserSingleCache.GetOrCreate(id,  () =>  new AdvertiserDal().GetById(id));
        }

        public  IEnumerable<Advertiser> GetAll()
        {
            return  _AdvertiserManyCache.GetOrCreate("__all__",  () =>  new AdvertiserDal().GetAll());
        }
    }
}
