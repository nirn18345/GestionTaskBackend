using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.Model.User
{
    public class UserRequest
    {
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string Dni { get; set; }

        public string? Phone { get; set; }
        public Guid AccountId { get; set; }
        public Guid RoleId { get; set; }

        public string CreatedBy { get; set; }


    }

    public class UpdateUserRequest
    {
        public Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dni { get; set; }
        public string Phone { get; set; }
        public Guid AccountId { get; set; }
        public Guid RoleId { get; set; }
        public string ModifiedBy { get; set; }


    }
}
