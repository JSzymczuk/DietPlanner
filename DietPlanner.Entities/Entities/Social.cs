using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietPlanner.Entities
{
    public class Rating
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }         // 1 do wielu
        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        [Required]
        public Guid RecipeId { get; set; }         // 1 do wielu
        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }

        [Required]
        public int Stars { get; set; }
    }

    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }         // 1 do wielu
        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        [Required]
        public Guid RecipeId { get; set; }          // 1 do wielu
        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }

        [Required]
        public DateTime Added { get; set; }

        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }

    public class Report
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        [Required]
        public Guid TargetId { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        public bool Checked { get; set; }
    }
}
