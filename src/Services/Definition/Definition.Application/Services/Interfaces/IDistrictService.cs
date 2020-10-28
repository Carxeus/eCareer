﻿using System.Threading.Tasks;
using Career.Cache.Attributes;
using Career.Cache.Helpers;
using Career.Utilities.Pagination;
using Definition.Application.Dtos;
using Definition.Application.Models.RequestModels;

namespace Definition.Application.Services.Interfaces
{
    public interface IDistrictService : IService
    {
        [Cache(TTL = 30 * TTLMultiplier.Day, SlidingExpiration = false)]
        Task<PagedList<DistrictDto>> GetAsync(PaginationFilter paginationFilter);
        
        [Cache(TTL = 30 * TTLMultiplier.Day, SlidingExpiration = false)]
        Task<DistrictDto> GetByIdAsync(string id);
        
        Task<DistrictDto> CreateAsync(DistrictRequestModel requestModel);
        Task<DistrictDto> UpdateAsync(string id, DistrictRequestModel requestModel);
        Task DeleteAsync(string id);
    }
}