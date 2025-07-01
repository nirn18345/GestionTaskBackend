using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.DTOs.Users
{
    public class UserListDto
    {
        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Dni { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }      
        public Guid AccountId { get; set; }
        public Guid RoleId { get; set; }
        public string? RoleName { get; set; }     
    }
}
