using FinancialSystem.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialSystem.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IAccountingService, AccountingService>();

            return services;
        }
    }
}
