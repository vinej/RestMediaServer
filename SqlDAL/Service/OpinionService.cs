using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;

// simple service for future use 
namespace SqlDAL.Service
{
    public class OpinionService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<Opinion>> _OpinionManyCache = new WaitToFinishMemoryCache<IEnumerable<Opinion>>();
        static readonly WaitToFinishMemoryCache<Opinion> _OpinionSingleCache = new WaitToFinishMemoryCache<Opinion>();

        public  long Insert(Opinion Opinion)
        {
            return  new OpinionDal().Insert(Opinion);
        }

        public  long Update(Opinion Opinion)
        {
            return  new OpinionDal().Update(Opinion);
        }

        public  long Delete(long id)
        {
            return  new OpinionDal().Delete(id);
        }

        public  Opinion GetById(long id)
        {
            return  _OpinionSingleCache.GetOrCreate(id,  () =>  new OpinionDal().GetById(id));
        }

        public  IEnumerable<Opinion> GetAll()
        {
            return  _OpinionManyCache.GetOrCreate("__all__",  () =>  new OpinionDal().GetAll());
        }
    }
}
