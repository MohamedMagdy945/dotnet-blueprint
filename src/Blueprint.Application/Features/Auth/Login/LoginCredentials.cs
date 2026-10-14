namespace Blueprint.Application.Features.Auth.Login;

public sealed class LoginCredentials
{
    public Guid Id { get; init; }
    public string Email { get; init; } = default!;
    public string PasswordHash { get; init; } = default!;
    public bool IsActive { get; init; }
    public List<string> Roles { get; init; } = [];
}