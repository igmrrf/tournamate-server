

namespace UseCase.DTOs
{
    public record ConfirmationTokenResponse
    {
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Token { get; set; } = default!;
        public Guid UserId { get; set; }
    }
}
