using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;

// simple service for future use 
namespace SqlDAL.Service
{
    public class TopicService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<Topic>> _TopicManyCache = new WaitToFinishMemoryCache<IEnumerable<Topic>>();
        static readonly WaitToFinishMemoryCache<Topic> _TopicSingleCache = new WaitToFinishMemoryCache<Topic>();

        public  long Insert(Topic Topic)
        {
            return  new TopicDal().Insert(Topic);
        }

        public  long Update(Topic Topic)
        {
            return  new TopicDal().Update(Topic);
        }

        public  long Delete(long id)
        {
            return  new TopicDal().Delete(id);
        }

        public  Topic GetById(long id)
        {
            return  _TopicSingleCache.GetOrCreate(id,  () =>  new TopicDal().GetById(id));
        }

        public  IEnumerable<Topic> GetAll()
        {
            return  _TopicManyCache.GetOrCreate("__all__",  () =>  new TopicDal().GetAll());
        }
    }
}
