using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Filenet.Apis.Entities;
using Microsoft.EntityFrameworkCore;

namespace Filenet.Apis.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext _context;
        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        //public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        //{
        //    var city = GetCity(cityId, false);
        //    city.PointsOfInterest.Add(pointOfInterest);
        //}

        public async Task AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCity(cityId, false);
            city.PointsOfInterest.Add(pointOfInterest);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CityExists(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task <IEnumerable<City>> GetCities()
        {
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();  //ToList is necessary
        }

        
        public async Task<City> GetCity(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }

            return await _context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }
        
                
        public async Task<PointOfInterest> GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return await _context.PointsOfInterest
               .Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();
        }

        


        //public IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        //{
        //    return _context.PointsOfInterest
        //                   .Where(p => p.CityId == cityId).ToList();
        //}

        public async Task <IEnumerable<PointOfInterest>> GetPointsOfInterestForCity(int cityId)
        {
            return await _context.PointsOfInterest
                           .Where(p => p.CityId == cityId).ToListAsync();
        }


        //public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        //{
        //    _context.PointsOfInterest.Remove(pointOfInterest);
        //}

        public async Task DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointsOfInterest.Remove(pointOfInterest);
            await _context.SaveChangesAsync();
        }
        

        //public bool Save()
        //{
        //    return (_context.SaveChanges() >= 0);
        //}

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
