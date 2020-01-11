using System.Net;
using System.Web.Http;
using RestMediaServer.Controllers;
using SqlDAL.Service;
using SqlDAL.Domain;
using WebApi.Jwt;
using System;
using WebApi.Jwt.Filters;

public class TokenController : ApiController
{
    // This is naive endpoint for demo, it should use Basic authentication
    // to provide token or POST request
    [AllowAnonymous]
    public MemberToken Get(string email, string password)
    {
        if (CheckUser(email, password))
        {
            var service = new MemberService();
            var member = service.GetByEmail(email);
            var token = new MemberToken()
            {
                Alias = member.Alias,
                Token = JwtManager.GenerateToken(email)
            };

            return token;
        }
        throw new HttpResponseException(HttpStatusCode.Unauthorized);
    }

    [AllowAnonymous]
    public bool Get(string token)
    {
        return JwtAuthenticationAttribute.ValidateOneToken(token);
    }

    [AllowAnonymous]
    public MemberToken Get(string alias, string email, string password)
    {
        var service = new MemberService();
        var member = service.GetByEmail(email);
        // email must be unique
        if (member != null && member.Id != -1)
        {
            throw new HttpResponseException(HttpStatusCode.Conflict);
        }
        // alias must be unique
        member = service.GetByAlias(alias);
        if (member != null && member.Id != -1)
        {
            throw new HttpResponseException(HttpStatusCode.Conflict);
        }
        member = new Member()
        {
            Alias = alias,
            Email = email,
            HashPassword = SecurePasswordHasher.Hash(password),
            IsActive = true,
            Dob = DateTime.Now
        };
        var status = service.Insert(member);
        if (status == -1)
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        var token = new MemberToken()
        {
            Alias = alias,
            Token = JwtManager.GenerateToken(email)
        };

        return token;
    }

    public static bool CheckUser(string email, string password)
    {
        var member = (new MemberService().GetByEmail(email));
        if (member == null || member.Id == -1) return false;
        return SecurePasswordHasher.Verify(password, member.HashPassword);
    }
}