using System.ComponentModel.DataAnnotations;
using DIY_STORE.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DIY_STORE.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Comment is required.")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Comment must be between 5 and 1000 characters.")]
        public string Comment { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsVerifiedPurchase { get; set; }

        public int ProductId { get; set; }
        public string UserId { get; set; } = string.Empty;

        [ValidateNever]
        public Product Product { get; set; } = null!;
        [ValidateNever]
        public ApplicationUser User { get; set; } = null!;
    }
}
