using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Domain.Entities
{
    public class Task
    {
        public Guid TaskId { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public Guid? AssignedTo { get; set; } 

        public string Status { get; set; } = "Pendiente"; 
        public DateTime? DueDate { get; set; }

        public string? Priority { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("AssignedTo")]
        public User? user { get; set; }
    }
}
