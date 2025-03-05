using System.ComponentModel.DataAnnotations;

namespace CloudFluffInc.Models;

public class Subscriber
{
    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 20 characters")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;
}
