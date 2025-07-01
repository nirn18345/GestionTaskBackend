using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Domain.Entities
{
    public class Role
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }

        public  bool IsActive{get;set;}

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
