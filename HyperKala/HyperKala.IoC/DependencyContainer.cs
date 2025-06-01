using HyperKala.Application.Interfaces;
using HyperKala.Application.Services;
using HyperKala.DataLayer.Repositories;
using HyperKala.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HyperKala.IoC
{
    public class DependencyContainer
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            #region Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IWalletService, WalletService>();

            #endregion

            #region Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            #endregion
        }
    }
}
