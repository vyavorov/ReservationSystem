using System.ComponentModel.DataAnnotations;
using static ReservationSystem.Common.EntityValidationConstants.Review;

public class ReviewFormViewModel
{
    public int LocationId { get; set; }

    [Required]
    [StringLength(ReviewCommentMaxLength, ErrorMessage = "The review cannot exceed 500 characters.")]
    [MinLength(ReviewCommentMinLength, ErrorMessage = "The review must be at least {1} characters.")]
    public string Comment { get; set; } = null!;
}