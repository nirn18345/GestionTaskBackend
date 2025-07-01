using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.DTOs
{
    public class CompanyDTOs
    {
        [Required(ErrorMessage = "The Company Name is required.")]
        [StringLength(100, ErrorMessage = "The Company Name cannot exceed 100 characters")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "The TaxId is required.")]
        [StringLength(13, ErrorMessage = "The TaxId  cannot exceed 13 characters")]
        public string TaxId { get; set; }


        [Required(ErrorMessage = "The Address is required.")]
        [StringLength(200, ErrorMessage = "The Address  cannot exceed 200 characters,")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Phone Name is required.")]
        [StringLength(20, ErrorMessage = "The Phone  cannot exceed 20 characters,")]
        public string Phone { get; set; }

        [StringLength(250, ErrorMessage = "The LogoUrl  cannot exceed 250 characters,")]
        public string? LogoUrl { get; set; }
        [StringLength(20, ErrorMessage = "The PrimaryColor  cannot exceed 20 characters,")]
        public string? PrimaryColor { get; set; }
        
        public string? LegalNotice { get; set; }
        [StringLength(300, ErrorMessage = "The CallbackUrl  cannot exceed 300 characters,")]
        public string? CallbackUrl { get; set; }

    }
}
