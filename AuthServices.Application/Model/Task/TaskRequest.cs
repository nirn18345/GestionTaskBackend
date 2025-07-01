using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.Model.Task
{
    public class TaskRequest
    {
       

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pendiente";
       

        public string? Priority { get; set; }

        public string? CreatedBy { get; set; }

       

    }

    public class UpdateTaskRequest
    {

        public Guid TaskId { get; set; }
        public string Title { get; set; } = null!;

        public string? Description { get; set; }


        public string Status { get; set; } = "Pendiente";

        public DateTime DueDate { get; set; }

        public Guid? AssignedTo { get; set; }

        public string? Priority { get; set; }

        public string? UpdatedBy { get; set; }



    }
}
