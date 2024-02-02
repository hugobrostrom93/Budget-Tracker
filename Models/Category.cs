using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace BudgetTrackerHugo.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "You must enter a title.")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        [Required(ErrorMessage = "You must enter an icon.")]
        public string Icon { get; set; } = "";

        [Column(TypeName = "nvarchar(10)")]
        public string Type { get; set; } = "Expense";

        [NotMapped]
        public string? TitleWithIcon
        {
            get
            {
                return Icon + " " + Title;
            }
        }
    }
}
