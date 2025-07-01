using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.DTOs
{
    public class ListDtos
    {

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }      // Texto de búsqueda
        public string? SortBy { get; set; }      // Campo por el que ordenar
        public bool SortDescending { get; set; } = false; // True para ordenar desc


    }


    public class ListUsetDtos
    {

        public Guid UserId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }      // Texto de búsqueda
        public string? SortBy { get; set; }      // Campo por el que ordenar
        public bool SortDescending { get; set; } = false; // True para ordenar desc


    }
}
