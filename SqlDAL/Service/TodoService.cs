using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;

// simple service for future use 
namespace SqlDAL.Service
{

    public class TodoService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<Todo>> _TodoManyCache = new WaitToFinishMemoryCache<IEnumerable<Todo>>();
        static readonly WaitToFinishMemoryCache<Todo> _TodoSingleCache = new WaitToFinishMemoryCache<Todo>();

        public  long Insert(Todo Todo)
        {
            return  new TodoDal().Insert(Todo);
        }

        public  long Update(Todo Todo)
        {
            return  new TodoDal().Update(Todo);
        }

        public  long Delete(long id)
        {
            return  new TodoDal().Delete(id);
        }

        public  Todo GetById(long id)
        {
            return  _TodoSingleCache.GetOrCreate(id,  () =>  new TodoDal().GetById(id));
        }

        public IEnumerable<Todo> GetByIsDone(bool isDone)
        {
            return  _TodoManyCache.GetOrCreate(isDone,  () =>  new TodoDal().GetByIsDone(isDone));
        }

        public IEnumerable<Todo> GetAll()
        {
            return  _TodoManyCache.GetOrCreate("__all__",  () =>  new TodoDal().GetAll());
        }
    }
}
