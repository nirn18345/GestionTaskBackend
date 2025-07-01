using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }

        public string? Dni {  get; set; }

        public string? Phone {  get; set; }
        public Guid AccountId { get; set; }
        public Guid RoleId { get; set; }

        public bool IsActive {  get; set; }

        public DateTime? CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public Role Role { get; set; }
        public Account Account { get; set; }
    }
}

