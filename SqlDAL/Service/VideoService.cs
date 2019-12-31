using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;

// simple service for future use 
namespace SqlDAL.Service
{
    public class VideoService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<Video>> _VideoManyCache = new WaitToFinishMemoryCache<IEnumerable<Video>>();
        static readonly WaitToFinishMemoryCache<Video> _VideoSingleCache = new WaitToFinishMemoryCache<Video>();

        public  long Insert(Video Video)
        {
            return  new VideoDal().Insert(Video);
        }

        public  long Update(Video Video)
        {
            return  new VideoDal().Update(Video);
        }

        public  long Delete(long id)
        {
            return  new VideoDal().Delete(id);
        }

        public  Video GetById(long id)
        {
            return  _VideoSingleCache.GetOrCreate(id,  () =>  new VideoDal().GetById(id));
        }

        public  IEnumerable<Video> GetAll()
        {
            return  _VideoManyCache.GetOrCreate("__all__",  () =>  new VideoDal().GetAll());
        }
    }
}
