using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Domain.Entities
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsActive { get; set; }

        public  DateTime CreatedAt {  get; set; }  

        public string CreatedBy { get; set; }


        public DateTime? UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public User User { get; set; }
    }
}
