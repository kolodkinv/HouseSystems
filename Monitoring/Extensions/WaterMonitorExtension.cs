using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Monitoring.Repositories;
using Monitoring.Repositories.EF;

namespace Monitoring.Extensions
{
    public static class WaterMonitorExtension
    {
        /// <summary>
        /// Метод позволяющий управлять зависимостями мониторинга водяных счетчиков
        /// </summary>
        /// <param name="services">Метод расширения для IServiceCollection</param>
        /// <typeparam name="TDbContext">Контекст БД для работы мониторинга</typeparam>
        public static void AddWaterMonitoring<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWorkEF>();
            services.AddScoped<IMonitor, WaterMonitor>();
            services.AddScoped<DbContext, TDbContext>();
        }
    }
}