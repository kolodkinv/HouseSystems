using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Monitoring.Repositories;
using Monitoring.Repositories.EF;

namespace HomeSystems.Monitoring.Extensions
{
    public static class WaterMonitorExtension
    {
        public static void AddWaterMonitoring<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWorkEF>();
            services.AddScoped<IMonitor, WaterMonitor>();
            services.AddScoped<DbContext, TDbContext>();
        }
    }
}