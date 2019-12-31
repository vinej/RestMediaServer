using System.Net;
using System.Web.Http;
using RestMediaServer.Controllers;
using SqlDAL.Service;

public class TokenController : ApiController
{
    // This is naive endpoint for demo, it should use Basic authentication
    // to provide token or POST request
    [AllowAnonymous]
    public string Get(string id, string type)
    {
        string email = id;
        string password = type;
        if (CheckUser(email, password))
        {
            return UserService.GenerateToken(email);
        }

        throw new HttpResponseException(HttpStatusCode.Unauthorized);
    }

    public static bool CheckUser(string email, string password)
    {
        var member = (new MemberService().GetByEmail(email));
        return SecurePasswordHasher.Verify(password, member.HashPassword);
    }
}