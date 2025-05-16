// Models/ApplicationUser.cs
using ArackiralamaProje.Models;
using Microsoft.AspNetCore.Identity;
using System;

public class ApplicationUser : IdentityUser
{
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public string ProfileImageUrl { get; set; }
    public Customer Customer { get; set; }
}
