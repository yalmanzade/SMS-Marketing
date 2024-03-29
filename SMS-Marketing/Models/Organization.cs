﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS_Marketing.Models;
[Index(nameof(Name), IsUnique = true)]
public class Organization
{
    [Key]
    public int Id { get; set; }

    [Required]

    public string Name { get; set; } = string.Empty;

    [Required]
    public string ManagerId { get; set; } = string.Empty;

    [DataType(DataType.PhoneNumber)]
    public string TwilioPhoneNumber { get; set; } = string.Empty;

    [Required]
    public string ManagerName { get; set; } = string.Empty;

    public string Users { get; set; } = string.Empty;
    public bool IsActive { get; set; } = false;
    public bool IsTwitter { get; set; } = false;
    public bool IsSMS { get; set; } = false;
    public bool IsFacebook { get; set; } = false;

    [NotMapped]
    public List<Group>? Groups { get; set; }
    [NotMapped]
    public List<AppUser>? AppUsers { get; set; }
    [NotMapped]
    public List<Post> RecentPosts = new();
    [NotMapped]
    public AppUser CurrentUser = new();
    [NotMapped]
    public string SharingUrl { get; set; }

    public string GetSharingUrl()
    {
        return SharingUrl.Replace("Organization", "Share");
    }
}
public class CustomerViewModel
{
    public CustomerViewModel(Organization? organization, List<Group>? groups, List<Customer>? customers)
    {
        Organization = organization;
        Groups = groups;
        Customers = customers;
    }
    public Organization? Organization { get; set; }
    public List<Group>? Groups { get; set; }
    public List<Customer>? Customers { get; set; }

}
