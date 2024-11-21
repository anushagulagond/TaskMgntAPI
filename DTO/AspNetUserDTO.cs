namespace TaskMgntAPI.DTO
{
    public class AspNetUserDTO
    {
        public string Id { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
        public string? PasswordHash { get; set; }
        public string UserName { get; set; } = null!;

        public string UserRole { get; set; }
    }
}

