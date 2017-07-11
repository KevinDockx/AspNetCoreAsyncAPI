using AutoMapper;
using Filenet.Apis.Models;
using Filenet.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filenet.Apis.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet()]
        public async Task <IActionResult> GetCities()
        {
            var cityEntities = await _cityInfoRepository.GetCities();
            var results = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);

            return Ok(results);
        }

        //[HttpGet()]
        //public async Task <IActionResult> GetCities()
        //{
        //    var cityEntities = await _cityInfoRepository.GetCities();
        //    var results = await Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);

        //    return Ok(results);
        //}




        //// GET api/products/1
        //public async Task<Product> Get(int id)
        //{
        //    using (var ctx = new NorthwindSlimContext())
        //    {
        //        Product product = await
        //            (from p in ctx.Products.Include("Category")
        //             where p.ProductId == id
        //             orderby p.ProductName
        //             select p).SingleOrDefaultAsync();
        //        return product;
        //    }
        //}


        [HttpGet("{id}")]
        public async Task <IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCity(id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                var cityResult = Mapper.Map<CityDto>(city); 
                return Ok(cityResult);
            }

            var cityWithoutPointsOfInterestResult = Mapper.Map<CityWithoutPointsOfInterestDto>(city);
            return Ok(cityWithoutPointsOfInterestResult);
        }
    }
}
