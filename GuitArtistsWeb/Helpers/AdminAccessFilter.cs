using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GuitArtistsWeb.Helpers
{
    public class AdminAccessFilter : ActionFilterAttribute
    {
        private const string AdminKey = "ваш_ключ_адміністратора";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var key = context.HttpContext.Request.Query["key"];
            if (key != AdminKey)
            {
                context.Result = new BadRequestResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
