using System.Collections.Generic;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;
using WebApi.Jwt.Filters;

namespace RestMediaServer.Controllers
{
    public class TodoController : ApiController
    {
        // GET api/Todo
        [JwtAuthentication]
        public IEnumerable<Todo> Get()
        {
            return  new TodoService().GetAll();
        }

        // GET api/Todo/id
        [JwtAuthentication]
        public Todo Get(long id)
        {
            return new TodoService().GetById(id);
        }

        // GET api/Todo/id/type
        [JwtAuthentication]
        public IEnumerable<Todo> Get(string id, string type)
        {
            switch(type)
            {
                case "isdone":
                    // id = true|false
                    bool isDone = bool.Parse(id);
                    return  new TodoService().GetByIsDone(isDone);
                default:
                    return new List<Todo>();

            }
        }

        [JwtAuthentication]
        public static long Post([FromBody]Todo Todo)
        {
            //  /api/Todo
            // need to register the Todo and generate a token
            // hash the password before inserting it into the database
            return  new TodoService().Insert(Todo);
        }

        // PUT api/values/5
        [JwtAuthentication]
        public static long Put([FromBody]Todo Todo)
        {
            return  new TodoService().Update(Todo);
        }

        // DELETE api/values/5
        [JwtAuthentication]
        public static long Delete(long id)
        {
            return  new TodoService().Delete(id);
        }

    }
}
