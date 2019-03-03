using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Monitoring.Contexts;
using Monitoring.Exceptions;
using Monitoring.Models.Buildings;
using Monitoring.Models.Meters;
using Monitoring.Repositories.EF;
using Xunit;

namespace Monitoring.Tests
{
    /// <summary>
    /// Тесты для класса мониторинга водяных счетчиков
    /// </summary>
    public class WaterMonitorTest
    {
        /// <summary>
        /// Изоляция контекста между тестами. Состояние БД не передается из одного теста в другой
        /// </summary>
        /// <returns></returns>
        private static DbContextOptions<MonitoringContextEF> CreateNewContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<MonitoringContextEF>();
            builder.UseInMemoryDatabase(databaseName: "crypto")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        [Fact]
        // Корректная регистрация строения
        public async Task AddBuildingTestSuccess()
        {
            using (var monitor = new WaterMonitor(new UnitOfWorkEF(new MonitoringContextEF(CreateNewContextOptions()))))
            {
                monitor.AddBuilding(new House { Address = "Lenina 1" });

                var houses = await monitor.GetAllBuildingsAsync(b => b.WaterMeter);
                // Возращается список строений
                Assert.IsAssignableFrom<IEnumerable<Building>>(houses); 
                // В списке одно здание
                Assert.Single(houses);   
                // У этого здания нет водяного счетчика 
                Assert.Null(houses.FirstOrDefault().WaterMeter);   
            }                        
        }

        [Fact]
        // Корректная регистрация водяного счетчика
        public void AddMeterTestSuccess()
        {
            using (var monitor = new WaterMonitor(new UnitOfWorkEF(new MonitoringContextEF(CreateNewContextOptions()))))
            {
                // Регистрация новом дома
                var newHouse = new House {Address = "Lenina 1"};
                monitor.AddBuilding(newHouse);

                // Регистрация водяного счетчика в новом доме
                var newWaterMeter = new WaterMeter
                {
                    BuildingId = newHouse.Id,
                    Id = 1,
                    Value = 1
                };
                monitor.AddMeter(newWaterMeter);

                // Получение полной информации о новом доме
                var house = monitor.GetBuilding(h => h.Id == newWaterMeter.BuildingId, w => w.WaterMeter);

                // В доме устновлен водяной счетчик
                Assert.NotNull(house.WaterMeter);
                // У этого счетчика правильные показатели
                Assert.Equal(house.WaterMeter.Id, newWaterMeter.Id);
                // У этого счетчика правильные показатели   
                Assert.Equal(house.WaterMeter.Value, newWaterMeter.Value);
            }
        }

        [Fact]
        // Регистрация счетчиков с одним номером в разных строениях
        public void AddMeterTestDuplicate()
        {
            using (var monitor = new WaterMonitor(new UnitOfWorkEF(new MonitoringContextEF(CreateNewContextOptions()))))
            {
                // Регистрация новых домов
                var newHouse1 = new House {Address = "Lenina 1"};
                var newHouse2 = new House {Address = "Lenina 2"};
                monitor.AddBuilding(newHouse1);
                monitor.AddBuilding(newHouse2);

                // Регистрация водяных счетчиков с одинаковыми номерами в новых домах
                var newWaterMeter1 = new WaterMeter
                {
                    BuildingId = newHouse1.Id,
                    Id = 1,
                    Value = 1
                };

                var newWaterMeter2 = new WaterMeter
                {
                    BuildingId = newHouse2.Id,
                    Id = 1,
                    Value = 1
                };

                monitor.AddMeter(newWaterMeter1);

                // Исключение, одинаковые номера счетчиков в разных домах
                Assert.Throws<InvalidOperationException>(() => monitor.AddMeter(newWaterMeter2));
                // Первый счетчик зарегистрировался
                Assert.NotNull(monitor.GetMeter(newWaterMeter1.Id));
                // Первый счетчик зарегистрировался с первым строением
                Assert.NotNull(monitor.GetBuilding(b => b.Id == newWaterMeter1.BuildingId, w => w.WaterMeter)
                    .WaterMeter);
                // Второго счетчик не зарегистрировася со вторым строением
                Assert.Null(monitor.GetBuilding(b => b.Id == newWaterMeter2.BuildingId, w => w.WaterMeter).WaterMeter);
            }
        }

        [Fact]
        // Регистрация счетчика, неподдерживаемого системой мониторинга
        public void AddMeterTestNotWater()
        {
            using (var monitor = new WaterMonitor(new UnitOfWorkEF(new MonitoringContextEF(CreateNewContextOptions()))))
            {
                // Регистрация новом дома
                var newHouse = new House {Address = "Lenina 1"};
                monitor.AddBuilding(newHouse);

                // Регистрация счетчика в новом доме
                var newMeter = new Meter
                {
                    BuildingId = newHouse.Id,
                    Id = 1,
                    Value = 1
                };
                // Убеждаемся что счетчика с таким номером не зарегистрировано в системе
                Assert.Null(monitor.GetMeter(newMeter.Id));
                // Исключение, установка счетчика не поддерживаемого системой мониторинга
                Assert.Throws<ArgumentException>(() => monitor.AddMeter(newMeter));
                // Убеждаемся что счетчик не зарегистрировался
                Assert.Null(monitor.GetMeter(newMeter.Id));
            }
        }

        [Fact]
        // Регистрация водяного счетчика в несуществующем строении
        public void AddMeterTestNotFoundBuilding()
        {
            using (var monitor = new WaterMonitor(new UnitOfWorkEF(new MonitoringContextEF(CreateNewContextOptions()))))
            {
                // Регистрация водяного счетчика в доме
                var newWaterMeter = new WaterMeter
                {
                    BuildingId = 1,
                    Id = 1,
                    Value = 1
                };

                // Убеждаемся что строение в котором региструем счетчик не существует
                Assert.Null(monitor.GetBuilding(newWaterMeter.BuildingId));
                // Исключение, дом в котором региструется счетчик не найден
                Assert.Throws<NotFoundException>(() => monitor.AddMeter(newWaterMeter));
                // Счетчик с несуществующим зданием не создается
                Assert.Null(monitor.GetMeter(newWaterMeter.Id));
                // Строение не создается
                Assert.Null(monitor.GetBuilding(newWaterMeter.BuildingId));
            }
        }
    }
}