using System.Collections.Generic;
using System.Web.Http;
using WebApi.Jwt.Filters;

namespace RestMediaServer.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [JwtAuthentication]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [JwtAuthentication]
        public string Get(int id)
        {
            return "value "+ id.ToString();
        }
    }
}
