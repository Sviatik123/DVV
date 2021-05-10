using System;
using System.ComponentModel.DataAnnotations;
using SubChoice.Core.Data.Entities;

namespace SubChoice.Core.Data.Dto
{
    public class SubjectData : BaseEntity
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Capacity")]
        public int StudentsLimit { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public int Id { get; set; }

        public Guid TeacherId { get; set; }
    }
}
