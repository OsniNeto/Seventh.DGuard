using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Seventh.DGuard.Business;
using Seventh.DGuard.Business.Interface;
using Seventh.DGuard.Repository;
using Seventh.DGuard.Repository.Interface;

namespace Seventh.DGuard
{
    public static class ServicesInject
    {
        public static void Execute(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region Business

            services.AddTransient<IServerBO, ServerBO>();
            services.AddTransient<IVideoBO, VideoBO>();
            services.AddTransient<IRecyclerStatusBO, RecyclerStatusBO>();

            #endregion

            #region Repositories

            services.AddTransient<IServerRepository, ServerRepository>();
            services.AddTransient<IVideoRepository, VideoRepository>();
            services.AddTransient<IRecyclerStatusRepository, RecyclerStatusRepository>();

            #endregion
        }
    }
}
