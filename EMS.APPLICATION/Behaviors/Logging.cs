using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EMS.APPLICATION.Behaviors
{
    public class Logging<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor, ILogsRepository auditLogRepository, UserManager<AppUserEntity> userManager) : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!typeof(TRequest).Name.EndsWith("Command"))
            {
                return await next();
            }

            var httpContext = httpContextAccessor.HttpContext;
            var claimsPrincipal = httpContext?.User;

            string? username = null;
            string? userId = null;

            if (claimsPrincipal?.Identity != null && claimsPrincipal.Identity.IsAuthenticated)
            {
                // Najpierw próbujemy pobraæ username z claimów
                username = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

                // Jeœli nie ma username, szukamy po email
                if (string.IsNullOrEmpty(username))
                {
                    var email = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                    if (!string.IsNullOrEmpty(email))
                    {
                        var appUserByEmail = await userManager.FindByEmailAsync(email);
                        username = appUserByEmail?.UserName;
                        userId = appUserByEmail?.Id;
                    }
                }
            }

            var ip = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            var ua = httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();

            TResponse? response = default;
            string status = "Success";

            try
            {
                response = await next();
            }
            catch (Exception ex)
            {
                status = "Error: " + ex.Message;
                throw;
            }
            finally
            {
                var audit = new LogEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Username = username,
                    Action = typeof(TRequest).Name,
                    RequestData = JsonSerializer.Serialize(request, new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles
                    }),
                    IpAddress = ip,
                    UserAgent = ua,
                    Status = status,
                    CreatedAt = DateTime.UtcNow
                    //JsonSerializer.Serialize zamienia obiekt C# w string w formacie JSON.
                };

                await auditLogRepository.AddAsync(audit);
            }

            return response!;
        }
    }
}