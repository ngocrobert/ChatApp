using API.Extensions;
using API.Interfaces;
using DatingApp2.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    /// <summary>
    /// cập nhật thông tin về thời điểm cuối cùng người dùng thực hiện một hành động trong hệ thống
    /// </summary>
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            //var username = resultContext.HttpContext.User.GetUsername();
            var userId = resultContext.HttpContext.User.GetUserId();

            //truy xuất dịch vụ IUserRepository từ RequestServices, cho phép ta tương tác với cơ sở dữ liệu liên quan đến người dùng
            //var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            var uow = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();

            //var user = await repo.GetUserByUsernameAsync(username);
            //var user = await repo.GetUserByIdAsync(userId);
            var user = await uow.UserRepository.GetUserByIdAsync(userId);


            user.LastActive = DateTime.UtcNow;
            await uow.Complete();
        }
    }
}
