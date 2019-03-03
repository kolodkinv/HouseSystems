using System;
using Microsoft.EntityFrameworkCore;
using Monitoring.Models.Buildings;
using Monitoring.Models.Meters;

namespace Monitoring.Repositories.EF
{
    /// <summary>
    /// Реализация патерна единицы работы для синхронизации контекстов бд в репозиториях
    /// </summary>
    public class UnitOfWorkEF : IUnitOfWork
    {
        private DbContext _context;
        private EFRepository<Meter> _meterRepository;
        private EFRepository<WaterMeter> _waterMeterRepository;
        private EFRepository<Building> _buildingRepository;
        private EFRepository<House> _houseRepository;
        
        public UnitOfWorkEF(DbContext context)
        {
            _context = context;
        }
        
        public IRepository<Meter> Meters
        {
            get { return _meterRepository ?? (_meterRepository = new EFRepository<Meter>(_context)); }
        }
        
        public IRepository<WaterMeter> WaterMeters
        {
            get { return _waterMeterRepository ?? (_waterMeterRepository = new EFRepository<WaterMeter>(_context)); }
        }
        
        public IRepository<Building> Buildings
        {
            get { return _buildingRepository ?? (_buildingRepository = new EFRepository<Building>(_context)); }
        }

        public IRepository<House> Houses
        {
            get { return _houseRepository ?? (_houseRepository = new EFRepository<House>(_context)); }
        }
        
        public void Save()
        {
            _context.SaveChanges();
        }
 
        private bool disposed = false;
 
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }
 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}