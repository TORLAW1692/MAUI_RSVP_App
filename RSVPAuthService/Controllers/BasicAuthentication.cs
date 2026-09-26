using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RSVPAuthService.DataAccess;
using System.Text;

namespace RSVPAuthService.Controllers;

public class BasicAuthentication : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string authorization =
            context.HttpContext.Request.Headers.Authorization.ToString();

        if (string.IsNullOrWhiteSpace(authorization))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        string[] headerParts = authorization.Split(' ');

        if (headerParts.Length != 2 ||
            !headerParts[0].Equals(
                "Basic",
                StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        try
        {
            string credentials =
                Encoding.UTF8.GetString(
                    Convert.FromBase64String(headerParts[1]));

            string[] credentialParts =
                credentials.Split(':', 2);

            if (credentialParts.Length != 2)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string email = credentialParts[0];
            string password = credentialParts[1];

            UserData userData = new();

            if (!userData.ValidateUser(email, password))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            base.OnActionExecuting(context);
        }
        catch
        {
            context.Result = new UnauthorizedResult();
        }
    }
}