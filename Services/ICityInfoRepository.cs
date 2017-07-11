using Filenet.Apis.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filenet.Apis.Services
{
    public interface ICityInfoRepository
    {
        //bool CityExists(int cityId);
        //IEnumerable<City> GetCities();
        //City GetCity(int cityId, bool includePointsOfInterest);
        //IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId);
        //PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);
        //void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);
        //void DeletePointOfInterest(PointOfInterest pointOfInterest);
        //bool Save();

        Task<bool> CityExists(int cityId);
        Task<IEnumerable<City>> GetCities();

        Task<City> GetCity(int cityId, bool includePointsOfInterest);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCity(int cityId);
        Task<PointOfInterest> GetPointOfInterestForCity(int cityId, int pointOfInterestId);
        Task AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);
        Task DeletePointOfInterest(PointOfInterest pointOfInterest);
        Task<bool> Save();
    }
}
