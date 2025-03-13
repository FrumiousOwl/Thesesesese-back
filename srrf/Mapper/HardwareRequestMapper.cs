using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using srrf.Models;
using srrf.Dto.HardwareRequest;
using srrf.Dto.Hardware;

namespace srrf.Mapper
{
    public static class HardwareRequestMapper
    {
        public static HardwareRequestDto ToHardwareRequestDto(this HardwareRequest hardwareRequestModel)
        {
            return new HardwareRequestDto
            {
                RequestId = hardwareRequestModel.RequestId,
                Name = hardwareRequestModel.Name,
                Department = hardwareRequestModel.Department,
                Workstation = hardwareRequestModel.Workstation,
                Problem = hardwareRequestModel.Problem,
                DateNeeded = hardwareRequestModel.DateNeeded,
                IsFulfilled = hardwareRequestModel.IsFulfilled,
                HardwareId = hardwareRequestModel.HardwareId,
                SerialNo = hardwareRequestModel.SerialNo,
                Status = hardwareRequestModel.Status
            };
        }
        public static HardwareRequest CreateHardwareRequestDto(this HardwareRequestCUDDto hardwareRequestDto)
        {
            return new HardwareRequest
            {
                Name = hardwareRequestDto.Name,
                Department = hardwareRequestDto.Department,
                Workstation = hardwareRequestDto.Workstation,
                Problem = hardwareRequestDto.Problem,
                DateNeeded = hardwareRequestDto.DateNeeded,
                IsFulfilled = hardwareRequestDto.IsFulfilled,
                HardwareId = hardwareRequestDto.HardwareId,
                SerialNo = hardwareRequestDto.SerialNo,
                Status = hardwareRequestDto.Status
            };
        }

    }
}
