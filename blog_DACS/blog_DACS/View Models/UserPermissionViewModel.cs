using System.ComponentModel.DataAnnotations;

namespace blog_DACS.View_Models
{
    public class UserPermissionViewModel
    {
        public long IdPermission { get; set; }

        public string NamePermission { get; set; } = null!;

        public string DescriptionPermission { get; set; } = null!;

        public long IdUser { get; set; }

        public string FullName { get; set; } = null!;

        public string? PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? PlaceOfBirth { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public string? ExistenceStatus { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Pass { get; set; } = null!;

        public long IdRole { get; set; }
    }
}
