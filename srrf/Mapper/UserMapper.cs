using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using srrf.Models;
using srrf.Dto.User;
using srrf.Dto.HardwareRequest;

namespace srrf.Mapper
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User userModel)
        {
            return new UserDto
            {
                UserId = userModel.UserId,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                Password = userModel.Password,
                PhoneNumber = userModel.PhoneNumber,
                Role = userModel.Role
            };
        }
        public static User CreateUserDto(this UserCRUD userDto)
        {
            return new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password,
                PhoneNumber = userDto.PhoneNumber,
                Role = userDto.Role
            };
        }
    }
}
