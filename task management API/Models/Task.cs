using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace task_management_API.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Required]
        [StringLength(50) , MinLength(5)]
        public string Name { get; set; }

        [Required]
        [StringLength(255), MinLength(10)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [ForeignKey("TeamMember")]
        public int MemberId { get; set; }

        public virtual TeamMember? TeamMember { get; set; }
    }
}
