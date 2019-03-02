using Monitoring.Models.Buildings;
using Monitoring.Models.Meters;

namespace Monitoring.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<Meter> Meters { get; }
        IRepository<WaterMeter> WaterMeters { get; }
        IRepository<Building> Buildings { get; }
        IRepository<House> Houses { get; }

        void Save();
    }
}