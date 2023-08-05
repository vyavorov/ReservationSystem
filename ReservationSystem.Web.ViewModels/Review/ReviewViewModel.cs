using System.ComponentModel.DataAnnotations;

public class ReviewViewModel
{
    [Required]
    public string Comment { get; set; } = null!;
    [Required]
    public string Username { get; set; } = null!;
}