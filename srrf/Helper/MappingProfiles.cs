﻿using srrf.Dto;
using srrf.Models;
using System.Diagnostics.Metrics;
using AutoMapper;
using srrf.Helper;

namespace srrf.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<ServiceRequest, ServiceRequestDto>();
            CreateMap<Invoice, InvoiceDto>();

            CreateMap<InvoiceDto, Invoice>();
            CreateMap<CategoryDto, Category>();
            CreateMap<ServiceRequestDto, ServiceRequest>();
        }
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {

        services.AddAutoMapper(typeof(MappingProfiles));
    }
}
