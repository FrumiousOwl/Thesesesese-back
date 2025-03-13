using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using srrf.Models;
using srrf.Dto.Hardware;
using srrf.Dto.HardwareRequest;

namespace srrf.Mapper
{
    public static class HardwareMapper
    {
        public static HardwareDto ToHardwareDto(this Hardware hardwareModel)
        {
            return new HardwareDto
            {
                HardwareId = hardwareModel.HardwareId,
                Name = hardwareModel.Name,
                Description = hardwareModel.Description,
                DatePurchased = hardwareModel.DatePurchased,
                Supplier = hardwareModel.Supplier,
                Defective = hardwareModel.Defective,
                Available = hardwareModel.Available,
                Deployed = hardwareModel.Deployed,
                TotalPrice = hardwareModel.TotalPrice
            };
        }
        public static Hardware CreateHardwareDto(this HardwareCUDDto hardwareDto)
        {
            return new Hardware
            {
                Name = hardwareDto.Name,
                Description = hardwareDto.Description,
                DatePurchased = hardwareDto.DatePurchased,
                Supplier = hardwareDto.Supplier,
                Defective = hardwareDto.Defective,
                Available = hardwareDto.Available,
                Deployed = hardwareDto.Deployed,
                TotalPrice = hardwareDto.TotalPrice
            };
        }
    }
}
