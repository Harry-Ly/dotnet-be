using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    // Validates request at the DTO level
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}