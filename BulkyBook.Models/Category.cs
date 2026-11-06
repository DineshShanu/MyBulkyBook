using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BulkyBook.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 10000, ErrorMessage = "Display Order must be between 1 and 10000!")]
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
    }
}
